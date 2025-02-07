<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Панель заказчика</h2>
    <ProductList @add-to-cart="addToCart" />
    <Cart ref="cart" />
  </div>
</template>
<script>
  import ProductList from '../components/ProductList.vue';
  import Cart from '../components/ShoppingCart.vue';
  export default {
    components: { ProductList, Cart },
    methods: {
      addToCart(product) {
        this.$refs.cart.addToCart(product);
      },
    },
  };
</script>
<style scoped>
  .container {
    padding: 20px;
  }
</style>
