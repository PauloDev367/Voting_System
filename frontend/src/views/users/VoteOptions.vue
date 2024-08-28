<template>
  <LoggedLayout>
    <main>
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-12 text-center mb-4">
            <template v-if="voteIsOpen">
              <h2 class="badge badge-success">Votação aberta</h2>
            </template>
            <template v-else>
              <h2 class="badge badge-danger">Votação está fechada</h2>
            </template>
            <h1>Escolha seu representante</h1>
          </div>
          <div class="col-12 col-md-6">
            <template v-if="agents.length > 0">
              <ul>
                <li v-for="agent in agents" :key="agent.id">
                  <span>
                    {{ agent.agentName }} - ({{ agent.totalVotes }} votos)
                  </span>

                  <template v-if="voteIsOpen">
                    <button
                      class="btn btn-sm btn-outline-info"
                      @click="vote(agent.agentId)"
                    >
                      <i class="fa-regular fa-square-check"></i>
                    </button>
                  </template>
                  <template v-else>
                    <span class="badge badge-danger">
                      <i class="fa-solid fa-ban"></i> Votação finalizada
                    </span>
                  </template>
                </li>
              </ul>
            </template>
          </div>

          <div class="col-12 text-center mt-4">
            <p>
              Um total de
              <strong>{{ totalVotes }}</strong>
              pessoas já votaram
            </p>
          </div>
        </div>
      </div>
    </main>
  </LoggedLayout>
</template>


<script>
import LoggedLayout from "../layouts/LoggedLayout.vue";

export default {
  name: "VoteOptions",
  components: { LoggedLayout },
  data() {
    return {
      connection: null,
      agents: [],
      voteIsOpen: false,
      totalVotes: 0,
    };
  },
  async mounted() {
    const signalR = require("@microsoft/signalr");

    const token = window.localStorage.getItem("token");

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5293/votes", {
        accessTokenFactory: () => token,
      })
      .build();

    await this.connection
      .start()
      .then(() => {
        this.connectionActive = true;
      })
      .catch((err) => console.error("Error while starting connection: ", err));

    if (this.connectionActive == true) {
      try {
        await this.connection.invoke("AddConnectionIdToUser");
        await this.loadVoteData();
        this.loadNewAgents();
        this.loadNewVoteStatus();
        this.loadWinner();
      } catch (error) {
        this.check401Error();
      }
    }
  },
  methods: {
    async vote(agentId) {
      const confirm = window.confirm("Deseja votar nesse representante?");
      if (confirm) {
        try {
          await this.connection.invoke("AddVoteAsync", {
            OptionVoted: agentId,
          });
        } catch (error) {
          this.check401Error();
        }
      }
    },
    loadWinner() {
      this.connection.on("WinnerSelected", (winner) => {
        alert(
          "Ganhador buscado" +
            winner.agentName +
            " com um total de " +
            winner.totalVotes +
            " votos"
        );
      });
    },
    loadNewVoteStatus() {
      this.connection.on("VoteStatusChanged", (data) => {
        this.voteIsOpen = data;
      });
    },
    async loadVoteData() {
      try {
        this.connection.invoke("GetTotalVotesPerAgentAsync");
        this.connection.on("TotalPerAgent", (data) => {
          this.agents = data;
        });
        await this.connection.invoke("GetVoteInfosToClientAsync");
        this.connection.on("LoadClientVoteInfo", (data) => {
          this.totalVotes = data.totalVotes;
          this.voteIsOpen = data.voteIsOpen;
        });
      } catch (error) {
        this.check401Error(error);
      }
    },
    loadNewAgents() {
      this.connection.on("NewAgentRegistered", (data) => {
        this.agents.push(data);
      });
    },
    check401Error(err) {
      if (err.message && err.message.includes("unauthorized")) {
        alert("Você não tem permissão para estar aqui");
        window.location.href = "/";
      }
    },
  },
};
</script>

<style scoped>
main {
  padding: 60px 0;
}

main h1 {
  font-size: 1.5rem;
  font-weight: bold;
  text-transform: uppercase;
}

main ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

main ul li {
  padding: 10px;
  background-color: #f7f7f7;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}
</style>