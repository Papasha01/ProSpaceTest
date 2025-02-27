<template>
  <div class="user-editor">
    <h3 class="heading">Редактирование пользователей</h3>
    <ul class="user-list">
      <li v-for="user in users" :key="user.id" class="user-item">
        <div class="user-info">
          <span>{{ user.id }}</span>
          <span>{{ user.login }}</span>
          <span>{{ user.role }}</span>
        </div>
        <div>
          <button @click="editUser(user)" class="edit-btn">Редактировать</button>
          <button @click="deleteUser(user.id)" class="delete-btn">Удалить</button>
        </div>
      </li>
    </ul>
    <button @click="addUser" class="add-btn">Добавить пользователя</button>
    <UserModal :showModal="isModalVisible"
               :user="currentUser"
               :customer="currentCustomer"
               @close="closeModal"
               @submit="handleSaveUser" />
  </div>
</template>

<script>
  import UserModal from './UserModal.vue';

  export default {
    components: { UserModal },
    data() {
      return {
        users: [],
        isModalVisible: false,
        currentUser: {},
        currentCustomer: {},
        isSaving: false
      };
    },
    methods: {
      async fetchUsers() {
        try {
          const response = await fetch('https://localhost:7077/api/User/GetAllUsers');
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          this.users = await response.json();
        } catch (error) {
          console.error('Ошибка загрузки пользователей:', error);
          alert('Не удалось загрузить пользователей.');
        }
      },
      addUser() {
        this.currentUser = {};
        this.currentCustomer = {};
        this.isModalVisible = true;
      },
      async editUser(user) {
        this.currentUser = { ...user };
        try {
          const response = await fetch(`https://localhost:7077/api/Customer/GetCustomerByUserId/${user.id}`);
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          this.currentCustomer = await response.json();
        } catch (error) {
          console.error('Ошибка загрузки данных о пользователе:', error);
          alert('Не удалось загрузить данные о пользователе.');
          this.currentCustomer = {};
        }
        this.isModalVisible = true;
      },
      async deleteUser(userId) {
        if (!confirm("Вы уверены, что хотите удалить пользователя?")) return;
        try {
          const response = await fetch(`https://localhost:7077/api/User/delete/${userId}`, {
            method: "DELETE",
            headers: { "accept": "*/*" }
          });
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          this.fetchUsers();
        } catch (error) {
          console.error("Ошибка удаления пользователя:", error);
          alert("Не удалось удалить пользователя.");
        }
      },
      closeModal() {
        this.isModalVisible = false;
        this.fetchUsers();
      },
      async handleSaveUser(userData) {
        if (this.isSaving) return;
        this.isSaving = true;
        try {
          const isEdit = !!userData.id;
          const userResponse = await fetch(isEdit ? `https://localhost:7077/api/User/UpdateUser?id=${userData.id}` : 'https://localhost:7077/api/User/CreateAccount', {
            method: isEdit ? 'PUT' : 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              login: userData.login,
              password: userData.password,
              isActive: userData.isActive,
              role: userData.role,
              name: userData.customer?.name,
              code: userData.customer?.code,
              address: userData.customer?.address,
              discount: userData.customer?.discount
            }),
          });
          if (!userResponse.ok) throw new Error(`Ошибка HTTP: ${userResponse.status}`);

          if (isEdit && userData.customer) {
            const customerResponse = await fetch(`https://localhost:7077/api/Customer/${userData.customer.id}`, {
              method: 'PUT',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify(userData.customer),
            });
            if (!customerResponse.ok) throw new Error(`Ошибка HTTP: ${customerResponse.status}`);
          }

          alert('Успешно сохранено');
        } catch (error) {
          console.error('Ошибка сохранения:', error);
          alert('Не удалось сохранить пользователя.');
        } finally {
          this.isSaving = false;
        }
      }
    },
    mounted() {
      this.fetchUsers();
    }
  };
</script>

<style scoped>
  .user-editor {
    padding: 20px;
    max-width: 800px;
    margin: 0 auto;
  }

  .heading {
    text-align: center;
    margin-bottom: 20px;
  }

  .user-list {
    list-style: none;
    padding: 0;
    margin-bottom: 20px;
  }

  .user-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px;
    border: 1px solid #ddd;
    margin-bottom: 10px;
    border-radius: 4px;
  }

  .user-info {
    display: flex;
    flex-direction: column;
    gap: 8px;
  }

  .button-group {
    display: flex;
    flex-direction: column;
    gap: 10px;
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
  }

    .delete-btn:hover {
      background-color: #c82333;
    }
</style>
