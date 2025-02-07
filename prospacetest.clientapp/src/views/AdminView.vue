<template>
  <div class="admin-dashboard container">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2 class="text-center">Панель менеджера</h2>
      <div>
        <button @click="logout" class="btn btn-danger ms-2">Выход</button>
      </div>
    </div>
    <nav class="mb-4">
      <button @click="activeTab = 'catalog'"
              :class="['btn', activeTab === 'catalog' ? 'btn-primary' : 'btn-outline-primary', 'me-2']">
        Редактирование каталога
      </button>
      <button @click="activeTab = 'users'"
              :class="['btn', activeTab === 'users' ? 'btn-primary' : 'btn-outline-primary', 'me-2']">
        Управление пользователями
      </button>
      <button @click="activeTab = 'orders'"
              :class="['btn', activeTab === 'orders' ? 'btn-primary' : 'btn-outline-primary', 'me-2']">
        Управление заказами
      </button>
    </nav>
    <component :is="currentTabComponent" />
  </div>
</template>

<script>
  import CatalogEditor from '../components/CatalogEditor.vue';
  import UserEditor from '../components/UserEditor.vue';
  import OrderManager from '../components/OrderManager.vue';

  export default {
    data() {
      return {
        activeTab: 'catalog',
      };
    },
    computed: {
      currentTabComponent() {
        const components = {
          catalog: CatalogEditor,
          users: UserEditor,
          orders: OrderManager,
        };
        return components[this.activeTab];
      },
    },
    methods: {
      logout() {
        this.$router.push('/login');
      },
    },
  };
</script>

<style scoped>
  .admin-dashboard {
    padding: 20px;
  }
</style>
