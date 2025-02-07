// store/index.js
import { createStore } from 'vuex';

export default createStore({
  state: {
    cart: [], // Состояние корзины
  },
  getters: {
    totalPrice(state) {
      return state.cart.reduce((sum, item) => sum + item.price, 0);
    },
  },
  mutations: {
    ADD_TO_CART(state, product) {
      state.cart.push(product);
    },
    REMOVE_FROM_CART(state, id) {
      state.cart = state.cart.filter((item) => item.id !== id);
    },
    CLEAR_CART(state) {
      state.cart = [];
    },
  },
  actions: {
    addToCart({ commit }, product) {
      commit('ADD_TO_CART', product);
    },
    removeFromCart({ commit }, id) {
      commit('REMOVE_FROM_CART', id);
    },
    clearCart({ commit }) {
      commit('CLEAR_CART');
    },
  },
});
