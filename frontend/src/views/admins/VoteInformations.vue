<template>
  <div class="container mt-2">
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <button
        class="navbar-toggler"
        type="button"
        data-toggle="collapse"
        data-target="#navbarSupportedContent"
        aria-controls="navbarSupportedContent"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav ml-auto">
          <li class="nav-item active">
            <a class="nav-link btn btn-sm btn-outline-danger" href="#">
              <i class="fa-solid fa-right-from-bracket"></i> Logout
            </a>
          </li>
        </ul>
      </div>
    </nav>
  </div>

  <main>
    <div class="container">
      <div class="row">
        <div class="col-12 col-md-6">
          <template v-if="agents.length > 0">
            <ul>
              <li v-for="agent in agents" :key="agent.id">
                <span
                  >{{ agent.agentName }} - ({{ agent.totalVotes }} votos)</span
                >
                <button class="btn btn-sm btn-outline-danger" @click="removeAgent(agent.agentId)">
                  <i class="fa-solid fa-trash-can"></i>
                </button>
              </li>
            </ul>
          </template>
          <button
            class="btn btn-sm btn-block btn-info"
            data-toggle="modal"
            data-target="#modalAddRepresent"
          >
            <i class="fa-regular fa-square-plus"></i> Adicionar representante
          </button>
        </div>
        <div class="col-12 col-md-6 text-center mt-4">
          <div class="row">
            <div class="col-12 col-md-6">
              <button class="btn btn-block btn-warning">
                <i class="fa-solid fa-lock"></i> Finalizar votação
              </button>
            </div>
            <div class="col-12 col-md-6">
              <button class="btn btn-block btn-success">
                <i class="fa-solid fa-lock-open"></i> Abrir votação
              </button>
            </div>
            <div class="col-12 col-md-6 mt-2">
              <button class="btn btn-block btn-info">
                <i class="fa-brands fa-creative-commons-zero"></i> Zerar votação
              </button>
            </div>
            <div class="col-12 col-md-6 mt-2">
              <button class="btn btn-block btn-secondary">
                <i class="fa-solid fa-trophy"></i> Anunciar ganhador
              </button>
            </div>
            <div class="col-12 mt-3">
              <ul>
                <li>Total usuários no sistema: {{ totalUsers }}</li>
                <li>Total usuários online: {{ totalUsersOnline }}</li>
                <li>Total usuários que votaram: {{ totalUsersThatVoted }}</li>
                <li>
                  Total usuários que não votaram: {{ totalUsersThatNotVoted }}
                </li>
              </ul>
            </div>
          </div>
        </div>
        <div class="col-12 text-center">
          <p class="mt-5">
            Um total de
            <strong>{{ totalVotes }}</strong>
            pessoas já votaram
          </p>
        </div>
      </div>
    </div>

    <div
      class="modal fade"
      id="modalAddRepresent"
      tabindex="-1"
      role="dialog"
      aria-labelledby="modalAddRepresentLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modalAddRepresentLabel">
              Adicionar representante
            </h5>
            <button
              type="button"
              class="close"
              data-dismiss="modal"
              aria-label="Close"
            >
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <FormRegisterAgentComponent
            :connection="connection"
          ></FormRegisterAgentComponent>
        </div>
      </div>
    </div>
  </main>
</template>


<script>
import FormRegisterAgentComponent from "@/components/FormRegisterAgentComponent.vue";

export default {
  name: "VoteInformations",
  components: { FormRegisterAgentComponent },
  data() {
    return {
      connection: null,
      totalVotes: 0,
      totalUsers: 0,
      totalUsersThatVoted: 0,
      totalUsersThatNotVoted: 0,
      totalUsersOnline: 0,
      agents: [],
      connectionActive: false,
    };
  },
  async mounted() {
    const signalR = require("@microsoft/signalr");

    const token = window.localStorage.getItem("token");

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5293/votes/admin", {
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
      this.connection.invoke("AddConnectionIdToUser");
      this.loadVotesData();
      this.loadTotalVotesPerAgent();
      this.loadNewAgent();
    }
  },
  methods: {
    vote() {
      const confirm = window.confirm("Deseja votar nesse representante?");
      if (confirm) {
        alert("Enviando voto");
      }
    },
    removeAgent(agentId) {
      const confirm = window.confirm("Deseja remover esse representante?");

      if (confirm) {
        console.log(agentId);
        this.connection.invoke("RemoveAgentAsync", agentId);
      }
    },
    loadNewAgent() {
      this.connection.on("NewAgentRegistered", (data) => {
        this.agents.push(data);
      });
    },
    loadVotesData() {
      this.connection.invoke("GetVoteInformationAsync");
      this.connection.on("LoadSystemData", (data) => {
        this.totalVotes = data.totalVotes;
        this.totalUsers = data.totalUsers;
        this.totalUsersThatVoted = data.totalUsersThatVoted;
        this.totalUsersThatNotVoted = data.totalUsersThatNotVoted;
        this.totalUsersOnline = data.totalUsersOnline;
      });
    },
    loadTotalVotesPerAgent() {
      this.connection.invoke("GetTotalVotesPerAgentAsync");
      this.connection.on("TotalPerAgent", (data) => {
        this.agents = data;
      });
    },
  },
};
</script>

<style scoped>
main {
  padding: 100px 0;
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