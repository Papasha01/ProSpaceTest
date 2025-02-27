<template>
  <div class="catalog-editor">
    <h3 class="heading">Редактирование каталога</h3>
    <ul class="product-list">
      <li v-for="product in products" :key="product.id" class="product-item">
        <span>{{ product.name }} - {{ product.price }} р.</span>
        <div>
          <button @click="editProduct(product)" class="edit-btn">
            Редактировать
          </button>
          <button @click="deleteProduct(product.id)" class="delete-btn">
            Удалить
          </button>
        </div>
      </li>
    </ul>
    <button @click="addProduct" class="add-btn">Добавить товар</button>

    <ProductModal :showModal="isModalVisible"
                  :product="currentProduct"
                  @close="closeModal"
                  @submit="handleSaveProduct" />
  </div>
</template>

<script>
  import ProductModal from './ProductModal.vue';

  export default {
    components: {
      ProductModal,
    },
    data() {
      return {
        isSaving: false,
        products: [],
        isModalVisible: false,
        currentProduct: {} 
      };
    },
    methods: {
      async deleteProduct(productId) {
  if (!confirm("Вы уверены, что хотите удалить этот товар?")) return;
  
  try {
    const response = await fetch(`https://localhost:7077/api/Item/${productId}`, {
      method: "DELETE",
      headers: { "accept": "*/*" }
    });

    if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);

    this.fetchProducts();
  } catch (error) {
    console.error("Ошибка удаления товара:", error);
    alert("Не удалось удалить товар.");
  }
},

      async fetchProducts() {
        try {
          const response = await fetch('https://localhost:7077/api/Item');
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          const data = await response.json();
          this.products = data;
        } catch (error) {
          console.error('Ошибка загрузки:', error);
          alert('Не удалось загрузить список товаров.');
        }
      },
      addProduct() {
        this.currentProduct = {};
        this.isModalVisible = true;
      },
      editProduct(product) {
        this.currentProduct = { ...product };
        this.isModalVisible = true;
      },
      closeModal() {
        this.isModalVisible = false;
        this.fetchProducts();
      },
      async handleSaveProduct(productData) {
        if (this.isSaving) return;
        this.isSaving = true;

        try {
          const isEdit = !!productData.id;
          const method = isEdit ? 'PUT' : 'POST';
          const url = isEdit
            ? `https://localhost:7077/api/Item/${productData.id}`
            : 'https://localhost:7077/api/Item';

          const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(productData),
          });

          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);

          alert('Successfully');

        } catch (error) {
          console.error('Ошибка сохранения:', error);
          alert('Не удалось сохранить товар.');
        } finally {
          this.isSaving = false;
        }
      }
    },
    mounted() {
      this.fetchProducts();
    }
  };
</script>

<style scoped>
  .catalog-editor {
    padding: 20px;
    max-width: 800px;
    margin: 0 auto;
  }

  .heading {
    text-align: center;
    margin-bottom: 20px;
  }

  .product-list {
    list-style: none;
    padding: 0;
    margin-bottom: 20px;
  }

  .product-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px;
    border: 1px solid #ddd;
    margin-bottom: 10px;
    border-radius: 4px;
  }

  .edit-btn {
    background-color: #ffc107;
    border: none;
    color: #000;
    padding: 5px 10px;
    border-radius: 4px;
    cursor: pointer;
  }

    .edit-btn:hover {
      background-color: #e0a800;
    }

  .add-btn {
    display: block;
    margin: 0 auto;
    background-color: #007bff;
    border: none;
    color: #fff;
    padding: 10px 20px;
    border-radius: 4px;
    cursor: pointer;
  }

    .add-btn:hover {
      background-color: #0056b3;
    }

  .delete-btn {
    background-color: #dc3545;
    border: none;
    color: #fff;
    padding: 5px 10px;
    border-radius: 4px;
    cursor: pointer;
    margin-left: 10px;
  }

    .delete-btn:hover {
      background-color: #c82333;
    }
  .delete-btn {
    background-color: #dc3545;
    border: none;
    color: #fff;
    padding: 5px 10px;
    border-radius: 4px;
    cursor: pointer;
    margin-left: 10px;
  }

    .delete-btn:hover {
      background-color: #c82333;
    }

</style>
