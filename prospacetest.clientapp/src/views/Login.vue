<template>
  <div class="login container">
    <div class="row justify-content-center">
      <div class="col-md-6 col-lg-4">
        <div class="card shadow">
          <div class="card-body">
            <h2 class="card-title text-center mb-4">Авторизация</h2>
            <form @submit.prevent="login">
              <div class="mb-3">
                <input v-model="username"
                       type="text"
                       class="form-control"
                       placeholder="Логин"
                       required />
              </div>
              <div class="mb-3">
                <input v-model="password"
                       type="password"
                       class="form-control"
                       placeholder="Пароль"
                       required />
              </div>
              <button type="submit" class="btn btn-primary w-100">Войти</button>
            </form>
            <p v-if="error" class="text-danger text-center mt-3">{{ error }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import { auth } from '../auth';
  export default {
    data() {
      return {
        username: '',
        password: '',
        error: '',
      };
    },
    methods: {
      async login() {
        try {
          const response = await fetch('https://localhost:7077/api/User/authenticate', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify({
              login: this.username,
              password: this.password,
            }),
          });

          if (!response.ok) {
            throw new Error('Неверный логин или пароль');
          }

          const data = await response.json();


          if (data.role === 'Admin') {
            auth.login('admin');
            this.$router.push('/admin');
          } else if (data.role === 'Customer') {
            const customerId = data.customerId
            const firstname = data.firstname
            localStorage.setItem('customerId', customerId);
            localStorage.setItem('firstname', firstname);
            console.log(data);
            auth.login('customer');
            this.$router.push('/customer');
          } else {
            throw new Error('Неизвестная роль пользователя');
          }
        } catch (err) {
          this.error = err.message || 'Ошибка авторизации';
        }
      },
    },
  };
</script>

<style scoped>
  .login {
    margin-top: 50px;
  }
</style>
