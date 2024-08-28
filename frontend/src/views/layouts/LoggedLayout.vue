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
            <a
              class="nav-link btn btn-sm btn-outline-danger"
              @click.prevent="logoutUser"
              href="#"
            >
              <i class="fa-solid fa-right-from-bracket"></i> Logout
            </a>
          </li>
        </ul>
      </div>
    </nav>
  </div>
  <slot></slot>
</template>


<script>
export default {
  name: "LoggedLayout",
  data() {
    return {
      connection: null,
    };
  },
  async mounted() {
    const signalR = require("@microsoft/signalr");

    const token = window.localStorage.getItem("token");

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5293/votes/", {
        accessTokenFactory: () => token,
      })
      .build();

    await this.connection
      .start()
      .then(() => {
        this.connectionActive = true;
      })
      .catch((err) => console.error("Error while starting connection: ", err));
  },
  methods: {
    logoutUser() {
      const confirm = window.confirm("Deseja sair da sua conta?");

      if (confirm) {
        if (this.connectionActive == true) {
          this.connection.invoke("Logout");
          this.connection.on("LogoutUser", () => {
            window.localStorage.removeItem("token");
            window.location.href = "/";
          });
        }
      }
    },
  },
};
</script>