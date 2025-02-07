<template>
  <div class="cart container mt-5">
    <h3 class="text-center mb-4">Корзина</h3>
    <div v-if="cart.length === 0" class="text-center text-muted">
      Ваша корзина пуста.
    </div>
    <div v-else>
      <div class="row mb-3 fw-bold">
        <div class="col-5">Товар</div>
        <div class="col-2 text-end">Количество</div>
        <div class="col-2 text-end">Цена за шт.</div>
        <div class="col-3 text-end">Общая цена</div>
      </div>
      <div class="list-group">
        <div v-for="item in cart" :key="item.id" class="list-group-item">
          <div class="row align-items-center">
            <div class="col-5">{{ item.name }}</div>
            <div class="col-2 text-end">
              <button @click="decreaseQuantity(item.id)" class="btn btn-sm btn-outline-secondary">-</button>
              <span class="mx-2">{{ item.quantity }}</span>
              <button @click="increaseQuantity(item.id)" class="btn btn-sm btn-outline-secondary">+</button>
            </div>
            <div class="col-2 text-end">{{ item.price }} ₽</div>
            <div class="col-3 text-end">{{ item.totalPrice }} ₽</div>
          </div>
        </div>
      </div>
      <div class="text-end mt-4">
        <h5>Итого: {{ totalPrice }} ₽</h5>
        <button @click="checkout" class="btn btn-success btn-lg">
          Оформить заказ
        </button>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    name: 'ShoppingCart',
    data() {
      return {
        cart: [],
      };
    },
    computed: {
      totalPrice() {
        return this.cart.reduce((sum, item) => sum + item.totalPrice, 0);
      },
    },
    methods: {
      addToCart(product) {
        const existingItem = this.cart.find((item) => item.id === product.id);
        if (existingItem) {
          existingItem.quantity += 1;
        } else {
          this.cart.push({
            ...product,
            quantity: 1,
            totalPrice: product.price,
          });
        }
        this.updateTotalPrices();
      },
      removeFromCart(id) {
        this.cart = this.cart.filter((item) => item.id !== id);
      },
      increaseQuantity(id) {
        const item = this.cart.find((item) => item.id === id);
        if (item) {
          item.quantity += 1;
          item.totalPrice = item.price * item.quantity;
        }
      },
      decreaseQuantity(id) {
        const item = this.cart.find((item) => item.id === id);
        if (item && item.quantity > 1) {
          item.quantity -= 1;
          item.totalPrice = item.price * item.quantity;
        } else if (item && item.quantity === 1) {
          this.removeFromCart(id);
        }
      },
      async checkout() {
        try {
          const customerId = localStorage.getItem('customerId');
          if (!customerId) {
            alert('Пользователь не авторизован!');
            return;
          }

          const orderData = {
            customerId: customerId,
            orderDate: new Date().toISOString(),
            shipmentDate: null,
            orderNumber: Math.floor(100000000 + Math.random() * 900000000),
            status: 'Pending', 
            orderItems: this.cart.map((item) => ({
              itemId: item.id,
              itemsCount: item.quantity,
            })),
          };
          console.log(JSON.stringify(orderData, null, 2));
          const response = await fetch('https://localhost:7077/api/Order', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(orderData),
          });
          
          if (!response.ok) throw new Error('Ошибка при оформлении заказа');

          this.cart = [];
          alert('Заказ успешно оформлен!');
        } catch (error) {
          alert('Ошибка при оформлении заказа: ' + error.message);
        }
      },
      updateTotalPrices() {
        this.cart.forEach((item) => {
          item.totalPrice = item.price * item.quantity;
        });
      },
    },
  };
</script>

<style scoped>
  .cart {
    max-width: 800px;
    margin: 0 auto;
  }

  .list-group-item {
    padding: 1rem;
  }

  .btn-success {
    font-size: 1.1rem;
    padding: 0.5rem 2rem;
  }
</style>
