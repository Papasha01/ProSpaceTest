<template>
  <div v-if="showModal" class="modal-overlay">
    <div class="modal-content">
      <div class="modal-body">
        <h4 class="modal-heading">
          {{ isEditing ? "Редактировать пользователя" : "Добавить нового пользователя" }}
        </h4>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>Логин</label>
            <input type="text" v-model="newUser.login" class="form-control" required />
          </div>
          <div class="form-group">
            <label>Пароль</label>
            <input type="password" v-model="newUser.password" class="form-control" required />
          </div>
          <div class="form-group">
            <label>Активен</label>
            <input type="checkbox" v-model="newUser.isActive" class="form-check-input" />
          </div>
          <div class="form-group">
            <label>Роль</label>
            <div>
              <input type="radio" id="admin" value="Admin" v-model="newUser.role" @change="onRoleChange">
              <label for="admin">Admin</label>
            </div>
            <div>
              <input type="radio" id="customer" value="Customer" v-model="newUser.role" @change="onRoleChange">
              <label for="customer">Customer</label>
            </div>
          </div>
          <div v-if="newUser.role === 'Customer'" class="customer-fields">
            <div class="form-group">
              <label>Имя</label>
              <input type="text" v-model="newUser.customer.name" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Код</label>
              <input type="text"
                     v-model="newUser.customer.code"
                     class="form-control"
                     :class="{ 'is-invalid': newUser.customer.code && !isCodeValid }"
                     required />
              <div v-if="newUser.customer.code && !isCodeValid" class="invalid-feedback">
                Код «ХХХХ-ГГГГ» где Х – число, ГГГГ – год
              </div>
            </div>
            <div class="form-group">
              <label>Адрес</label>
              <input type="text" v-model="newUser.customer.address" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Скидка</label>
              <input type="number" v-model.number="newUser.customer.discount" class="form-control" min="0" required />
            </div>
          </div>
          <div class="form-actions">
            <button type="button" @click="closeModal" class="btn btn-secondary">
              Отмена
            </button>
            <button type="submit" class="btn btn-primary" :disabled="!isCodeValid">
              {{ isEditing ? "Сохранить" : "Добавить" }}
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
      showModal: Boolean,
      user: Object,
      customer: Object
    },
    data() {
      return {
        newUser: {
          login: "",
          password: "",
          isActive: true,
          role: "Customer",
          customer: {
            name: "",
            code: "",
            address: "",
            discount: 0
          }
        }
      };
    },
    computed: {
      isEditing() {
        return !!this.user?.id;
      },
      isCodeValid() {
        const pattern = /^\d{4}-(19\d{2}|20\d{2}|2100)$/;
        return pattern.test(this.newUser.customer.code);
      }
    },
    watch: {
      user: {
        immediate: true,
        handler(newUser) {
          this.newUser = newUser ? { ...newUser, customer: this.customer || {} } : {
            login: "",
            password: "",
            isActive: true,
            role: "Customer",
            customer: {
              name: "",
              code: "",
              address: "",
              discount: 0
            }
          };
        }
      }
    },
    methods: {
      closeModal() {
        this.$emit("close");
      },
      handleSubmit() {
        if (this.isCodeValid || this.newUser.role === "Admin") {
          this.$emit("submit", this.newUser);
        }
      },
      onRoleChange() {
        if (this.newUser.role === "Admin") {
          this.newUser.customer = {
            name: "",
            code: "",
            address: "",
            discount: 0
          };
        }
      }
    }
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
  }

  .is-invalid {
    border-color: red;
  }

  .invalid-feedback {
    color: red;
    font-size: 12px;
  }

  .customer-fields {
    margin-top: 20px;
  }
</style>
