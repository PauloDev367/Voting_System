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
      <div class="row justify-content-center">
        <div class="col-12 text-center mb-4">
          <h2 class="badge badge-success">Votação aberta</h2>
          <h1>Escolha seu representante</h1>
        </div>
        <div class="col-12 col-md-6">
          <template v-if="agents.length > 0">
            <ul>
              <li v-for="agent in agents" :key="agent.id">
                <span>
                  {{ agent.agentName }} - ({{ agent.totalVotes }} votos)
                </span>
                <button
                  class="btn btn-sm btn-outline-info"
                  @click="vote(agent.agentId)"
                >
                  <i class="fa-regular fa-square-check"></i>
                </button>
              </li>
            </ul>
          </template>
        </div>

        <div class="col-12 text-center mt-4">
          <p>
            Um total de
            <strong>123</strong>
            pessoas já votaram
          </p>
        </div>
      </div>
    </div>
  </main>
</template>


<script>
export default {
  name: "VoteOptions",
  data() {
    return {
      connection: null,
      agents: [],
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
      this.loadVoteData();
      this.loadNewAgents();
    }
  },
  methods: {
    vote() {
      const confirm = window.confirm("Deseja votar nesse representante?");
      if (confirm) {
        alert("Enviando voto");
      }
    },
    loadVoteData() {
      this.connection.invoke("GetTotalVotesPerAgentAsync");
      this.connection.on("TotalPerAgent", (data) => {
        this.agents = data;
      });
    },
    loadNewAgents() {
      this.connection.on("NewAgentRegistered", (data) => {
        this.agents.push(data);
      });
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