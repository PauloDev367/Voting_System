<template>
  <main>
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="card">
            <div class="card-header">
              <h1>Login</h1>
            </div>
            <div class="card-body">
              <form @submit.prevent="login">
                <div class="form-group">
                  <label for="email">E-mail:</label>
                  <input type="email" v-model="email" class="form-control" id="email" aria-describedby="emailHelp"
                    placeholder="Enter email">
                </div>
                <div class="form-group">
                  <label for="senha">Senha:</label>
                  <input type="password" v-model="password" class="form-control" id="senha" placeholder="Password">
                </div>

                <div class="text-right">
                  <button type="submit" class="btn btn-info">Entrar</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<script>

import { login } from '@/services/user.js';
export default {
  data() {
    return {
      email: '',
      password: ''
    }
  },
  name: 'HomeView',

  methods: {
    login() {
      if (this.email == '' || this.password == '') {
        alert("É preciso informar o e-mail e a senha");
      } else {
        login(this.email, this.password).then((response) => {
          window.localStorage.setItem("token", response.data.token);
          window.location.href = "/vote"
        }).catch((error) => {
          alert("Erro ao tentar fazer login");
          console.log('Erro:', error);
        });
      }
    }
  },
}
</script>

<style scoped>
main {
  margin-top: 200px;
}

h1 {
  font-size: 1.3rem;
  margin: 0;
  font-weight: bold;
}
</style>