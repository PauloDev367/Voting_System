import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import VoteOptions from '../views/users/VoteOptions.vue'
import VoteInformations from '../views/admins/VoteInformations.vue'
import { getUserRole, verifyIfIsLogged } from '@/auth'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/vote',
    name: 'vote',
    component: VoteOptions,
    beforeEnter: (to, from, next) => {
      verifyIfIsLogged(to, from, next);
      const userRole = getUserRole();

      if (!userRole.includes("CLIENT")) {
        alert("Você não tem permissão para acessar essa rota");
        next("/");
      }
      next();
    }
  },
  {
    path: '/admin/vote',
    name: 'admin-vote',
    component: VoteInformations,
    beforeEnter: (to, from, next) => {
      verifyIfIsLogged(to, from, next);
      const userRole = getUserRole();

      if (!userRole.includes("ADMIN")) {
        alert("Você não tem permissão para acessar essa rota");
        next("/");
      }
      next();
    }
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
