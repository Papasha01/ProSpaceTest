<template>
  <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <slot></slot>
      </div>
    </div>
  </div>

</template>

<script>
  export default {
    props: {
      show: Boolean,
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
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
  }

  .modal-dialog {
    max-width: 500px;
    width: 90%;
  }
</style>
