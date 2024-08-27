<template>
  <form @submit.prevent="registerAgent">
    <div class="modal-body">
      <div class="form-group">
        <label for="">Nome:</label>
        <input type="text" v-model="nome" class="form-control" />
      </div>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-sm btn-secondary"
        data-dismiss="modal"
      >
        Fechar
      </button>
      <button type="submit" class="btn btn-sm btn-info">
        <i class="fa-regular fa-square-plus"></i> Adicionar
      </button>
    </div>
  </form>
</template>

<script>
export default {
  name: "FormRegisterAgentComponent",
  props: {
    connection: { required: true },
  },
  data() {
    return {
      nome: "",
    };
  },
  methods: {
    registerAgent() {
      this.connection
        .invoke("AddNewAgent", { AgentName: this.nome })
        .then(() => {
          this.nome = "";
        })
        .catch((err) => {
          console.log(err);
        });
    },
  },
};
</script>