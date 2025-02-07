import { createRouter, createWebHistory } from 'vue-router';
import { auth } from './auth';
import Login from './views/Login.vue';
import AdminView from './views/AdminView.vue';
import CustomerView from './views/CustomerView.vue';

const routes = [
  {
    path: '/',
    redirect: '/login' 
  },
  { path: '/login', component: Login },
  {
    path: '/admin',
    component: AdminView,
    meta: { requiresAuth: true, role: 'admin' },
  },
  {
    path: '/customer',
    component: CustomerView,
    meta: { requiresAuth: true, role: 'customer' },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth) {
    if (!auth.isAuthenticated) {
      next('/');
    } else if (to.meta.role && !auth.hasRole(to.meta.role)) {
      next('/');
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
