<template>
  <div class="product-list container mt-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h3 class="mb-0">Каталог товаров</h3>
      <div class="d-flex gap-2">
        <select v-model="sortBy" class="form-select">
          <option value="name">Сортировать по умолчанию</option>
          <option value="priceUp">Сортировать по возрастанию цены</option>
          <option value="priceDown">Сортировать по убыванию цены</option>
        </select>
      </div>
    </div>
    <div class="category-filters mb-4">
      <button v-for="category in categories"
              :key="category"
              @click="toggleCategory(category)"
              class="btn btn-outline-primary btn-sm me-2"
              :class="{ 'active': selectedCategory === category }">
        {{ category || 'Без категории' }}
      </button>
    </div>
    <div v-if="loading" class="text-center">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Загрузка...</span>
      </div>
    </div>
    <div v-if="error" class="alert alert-danger text-center">
      {{ error }}
    </div>
    <div v-if="!loading && !error">
      <div v-if="sortedProducts.length === 0" class="text-center text-muted">
        Товаров в выбранной категории нет
      </div>
      <ul v-else class="list-group">
        <li v-for="product in sortedProducts"
            :key="product.id"
            class="list-group-item d-flex justify-content-between align-items-center">
          <div class="me-3">
            <strong>{{ product.name }}</strong>
            <div class="text-muted small">
              <span v-if="product.category">Категория: {{ product.category }}</span>
              <span v-else>Без категории</span>
            </div>
          </div>
          <div class="text-end">
            <div class="h5 mb-2">{{ product.price }} ₽</div>
            <button @click="addToCart(product)"
                    class="btn btn-sm btn-success">
              В корзину
            </button>
          </div>
        </li>
      </ul>
    </div>
  </div>

  <div class="user-panel position-fixed top-0 end-0 m-3 text-end">

      <span class="badge bg-primary">{{firstName}}</span>

    <button @click="logout" class="btn btn-danger btn-sm">Выход</button>
  </div>
</template>

<script>
  export default {
    data() {

      return {
        products: [],
        loading: true,
        error: null,
        sortBy: 'name',
        selectedCategory: null,
        categories: [],
        firstName: ''
      };
    },
    computed: {
      filteredProducts() {
        if (!this.selectedCategory) return this.products;
        return this.products.filter(p => p.category === this.selectedCategory);
      },
      sortedProducts() {
        return [...this.filteredProducts].sort((a, b) => {
          if (this.sortBy === 'priceUp') { return a.price - b.price; }
          if (this.sortBy === 'priceDown') { return b.price - a.price; }
          return a.name.localeCompare(b.name);
        });
      }
    },
    methods: {
      addToCart(product) {
        this.$emit('add-to-cart', product);
      },
      toggleCategory(category) {
        this.selectedCategory = this.selectedCategory === category ? null : category;
      },
      async fetchProducts() {
        try {
          const response = await fetch('https://localhost:7077/api/Item');
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          const data = await response.json();
          this.products = data;

          const categories = [...new Set(data.map(p => p.category))];
          this.categories = categories.filter(c => c);
        } catch (error) {
          console.error('Ошибка загрузки:', error);
          this.error = 'Не удалось загрузить товары. Попробуйте позже.';
        } finally {
          this.loading = false;
        }
      },
      logout() {
        localStorage.removeItem('firstname');
        this.$router.push('/login');
      }
    },
    mounted() {
      const storedFirstName = localStorage.getItem('firstname');
      if (!storedFirstName) {
        alert('Пользователь не авторизован!');
        this.$router.push('/login');
        return;
      }
      else {
        this.firstName = storedFirstName
      }

      this.fetchProducts();
    }
  };
</script>

<style scoped>
  .footer {
    border-top: 1px solid #ddd;
  }

  .user-panel {
    z-index: 1000;
  }

  .list-group-item {
    transition: transform 0.2s;
  }

    .list-group-item:hover {
      transform: translateX(5px);
    }

  .btn.active {
    background-color: #0d6efd;
    color: white;
  }

  .form-select {
    max-width: 250px;
  }
</style>
