<template>
  <div v-if="showModal" class="modal-overlay">
    <div class="modal-content">
      <div class="modal-body">
        <h4 class="modal-heading">
          {{ isEditing ? 'Редактировать товар' : 'Добавить новый товар' }}
        </h4>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>Код товара</label>
            <input type="text" v-model="newProduct.code" required />
          </div>
          <div class="form-group">
            <label>Название</label>
            <input type="text" v-model="newProduct.name" required />
          </div>
          <div class="form-group">
            <label>Цена</label>
            <input type="number"
                   v-model.number="newProduct.price"
                   min="0"
                   step="0.01"
                   required />
          </div>
          <div class="form-group">
            <label>Категория</label>
            <input type="text" v-model="newProduct.category" required />
          </div>
          <div class="form-actions">
            <button type="button" @click="closeModal" class="cancel-btn">
              Отмена
            </button>
            <button type="submit" class="submit-btn">
              {{ isEditing ? 'Сохранить' : 'Добавить' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    props: {
      showModal: {
        type: Boolean,
        default: false,
      },
      product: {
        type: Object,
        default: () => ({}),
      },
    },
    data() {
      return {
        newProduct: {
          code: '',
          name: '',
          price: 0,
          category: '',
        },
      };
    },
    computed: {
      isEditing() {
        return this.product && this.product.id;
      },
    },
    watch: {
      product: {
        immediate: true,
        handler(newVal) {
          if (newVal && Object.keys(newVal).length > 0) {
            this.newProduct = { ...newVal };
          } else {
            this.resetForm();
          }
        },
      },
    },
    methods: {
      closeModal() {
        this.$emit('close');
        this.resetForm();
      },
      handleSubmit() {
        if (this.isFormValid()) {
          this.$emit('submit', { ...this.newProduct });
        }
      },
      isFormValid() {
        return Object.values(this.newProduct).every((field) => {
          if (typeof field === 'number') return field >= 0;
          return field.trim() !== '';
        });
      },
      resetForm() {
        this.newProduct = {
          code: '',
          name: '',
          price: 0,
          category: '',
        };
      },
    },
  };
</script>

<style scoped>
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
  }

  .modal-content {
    background: #fff;
    padding: 20px;
    border-radius: 8px;
    width: 90%;
    max-width: 500px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  }

  .modal-body {
    padding: 10px;
  }

  .modal-heading {
    margin-bottom: 15px;
    text-align: center;
  }

  .form-group {
    margin-bottom: 15px;
    display: flex;
    flex-direction: column;
  }

    .form-group label {
      margin-bottom: 5px;
      font-weight: bold;
    }

    .form-group input {
      padding: 8px;
      border: 1px solid #ccc;
      border-radius: 4px;
    }

  .form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
  }

  .cancel-btn {
    background-color: #6c757d;
    border: none;
    color: #fff;
    padding: 8px 16px;
    border-radius: 4px;
    cursor: pointer;
  }

    .cancel-btn:hover {
      background-color: #5a6268;
    }

  .submit-btn {
    background-color: #007bff;
    border: none;
    color: #fff;
    padding: 8px 16px;
    border-radius: 4px;
    cursor: pointer;
  }

    .submit-btn:hover {
      background-color: #0056b3;
    }
</style>
