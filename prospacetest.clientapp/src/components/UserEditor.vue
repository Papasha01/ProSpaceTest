<template>
  <div class="user-editor">
    <h3 class="heading">Редактирование пользователей</h3>
    <ul class="user-list">
      <li v-for="user in users" :key="user.id" class="user-item">
        <span>{{ user.name }} - {{ user.code }}</span>
        <div>
          <button @click="editUser(user)" class="edit-btn">Редактировать</button>
          <button @click="deleteUser(user.id)" class="delete-btn">Удалить</button>
        </div>
      </li>
    </ul>
    <button @click="addUser" class="add-btn">Добавить пользователя</button>

    <UserModal :showModal="isModalVisible"
               :user="currentUser"
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
        isSaving: false
      };
    },
    methods: {
      async fetchUsers() {
        try {
          const response = await fetch('https://localhost:7077/api/Customer');
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          this.users = await response.json();
        } catch (error) {
          console.error('Ошибка загрузки пользователей:', error);
          alert('Не удалось загрузить пользователей.');
        }
      },
      addUser() {
        this.currentUser = {};
        this.isModalVisible = true;
      },
      editUser(user) {
        this.currentUser = { ...user };
        this.isModalVisible = true;
      },
      async deleteUser(userId) {
        if (!confirm("Вы уверены, что хотите удалить пользователя?")) return;
        try {
          const response = await fetch(`https://localhost:7077/api/Customer/${userId}`, {
            method: "DELETE",
            headers: { "accept": "*/*" }
          });
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          this.fetchUsers();
        } catch (error) {
          console.error("Ошибка удаления пользователя:", error);
          alert("Эти пользователи связаны с таблицей User, и я не стал делать каскадное удаление, потому что для этого надо добавлять работу API с user, а время 4:44 утра, сдавать тестовое через 6 часов, а мне еще на завод вставать в 7.40. В общем косяк признаю.");
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
          const method = isEdit ? 'PUT' : 'POST';
          const url = isEdit ? `https://localhost:7077/api/Customer/${userData.id}` : 'https://localhost:7077/api/Customer';
          const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData),
          });
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
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
</style>
