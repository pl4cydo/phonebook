import { createRouter, createWebHistory } from 'vue-router'
import AppWrapper from '@/components/AppWrapper.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: AppWrapper, // Apontando para AppWrapper
    },
  ],
})

export default router
