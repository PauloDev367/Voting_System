import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import VoteOptions from '../views/users/VoteOptions.vue'
import VoteInformations from '../views/admins/VoteInformations.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/vote',
    name: 'vote',
    component: VoteOptions
  },
  {
    path: '/admin/vote',
    name: 'admin-vote',
    component: VoteInformations
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
