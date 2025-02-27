<template>
  <div class="order-manager">
    <h3 class="heading">Управление заказами</h3>
    <div class="filter">
      <label for="status-filter" class="filter-label">Фильтр по статусу:</label>
      <select id="status-filter" v-model="statusFilter" class="filter-select">
        <option value="">Все</option>
        <option value="Pending">В обработке</option>
        <option value="Done">Выполнен</option>
      </select>
    </div>
    <div class="sort">
      <label for="sort-order" class="sort-label">Сортировать по дате:</label>
      <select id="sort-order" v-model="sortOrder" class="sort-select">
        <option value="desc">Сначала новые</option>
        <option value="asc">Сначала старые</option>
      </select>
    </div>
    <ul class="order-list">
      <li v-for="order in filteredOrders" :key="order.id" class="order-item">
        <div class="order-info">
          <strong>Заказ №{{ order.orderNumber || order.id }}</strong>
          <span class="order-status">{{ order.status }}</span>
          <small class="order-date">от {{ formatDate(order.orderDate) }}</small>
          <small v-if="order.status === 'Done'" class="shipment-date">
            Отгружено: {{ formatDate(order.shipmentDate) }}
          </small>
        </div>
        <div class="order-actions">
          <span v-if="order.status !== 'Pending'"
                @click="updateOrderStatus(order, 'Pending')"
                class="return-text">
            Вернуть в обработку
          </span>
          <button v-if="order.status !== 'Done'"
                  @click="updateOrderStatus(order, 'Done')"
                  class="btn done-btn">
            Завершить
          </button>
          <span v-if="order.status === 'Done'"
                @click="deleteOrder(order.id)"
                class="delete-text">
            Удалить
          </span>
        </div>
      </li>
    </ul>
  </div>
</template>

<script>
  export default {
    name: "OrderManager",
    data() {
      return {
        orders: [],
        statusFilter: "",
        sortOrder: "desc",
        isUpdating: false
      };
    },
    computed: {
      filteredOrders() {
        let filtered = this.orders;
        if (this.statusFilter) {
          filtered = filtered.filter(order => order.status === this.statusFilter);
        }
        return filtered.sort((a, b) => {
          return this.sortOrder === "desc"
            ? new Date(b.orderDate) - new Date(a.orderDate)
            : new Date(a.orderDate) - new Date(b.orderDate);
        });
      }
    },
    methods: {
      async fetchOrders() {
        try {
          const response = await fetch("https://localhost:7077/api/Order");
          if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
          this.orders = await response.json();
        } catch (error) {
          console.error("Ошибка загрузки заказов:", error);
          alert("Не удалось загрузить список заказов.");
        }
      },
      formatDate(dateStr) {
        if (!dateStr || new Date(dateStr).getTime() === 0) return "Не назначено";
        return new Date(dateStr).toLocaleString();
      },
      async updateOrderStatus(order, newStatus) {
        if (this.isUpdating) return;
        this.isUpdating = true;

        try {
          const updatedOrder = {
            ...order,
            status: newStatus,
            shipmentDate: newStatus === "Done" ? new Date().toISOString() : null
          };
          const url = `https://localhost:7077/api/Order/${order.id}`;
          const response = await fetch(url, {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
              accept: "*/*"
            },
            body: JSON.stringify(updatedOrder)
          });
          console.log(JSON.stringify(updatedOrder));

          if (!response.ok) {
            throw new Error(`Ошибка HTTP: ${response.status}`);
          }

          await this.fetchOrders();
        } catch (error) {
          console.error("Ошибка обновления заказа:", error);
          alert("Не удалось обновить заказ.");
        } finally {
          this.isUpdating = false;
        }
      },
      async deleteOrder(orderId) {
        if (!confirm("Вы уверены, что хотите удалить этот заказ?")) return;
        try {
          const url = `https://localhost:7077/api/Order/${orderId}`;
          const response = await fetch(url, {
            method: "DELETE",
            headers: {
              accept: "*/*"
            }
          });
          if (!response.ok) {
            throw new Error(`Ошибка HTTP: ${response.status}`);
          }
          await this.fetchOrders();
        } catch (error) {
          console.error("Ошибка удаления заказа:", error);
          alert("Не удалось удалить заказ.");
        }
      }
    },
    mounted() {
      this.fetchOrders();
    }
  };
</script>

<style scoped>
  .done-btn {
    background-color: #6c757d; /* Темно-серый */
    color: #fff;
  }

    .done-btn:hover {
      background-color: #5a6268;
    }

  .order-manager {
    padding: 20px;
    max-width: 800px;
    margin: 0 auto;
    font-family: sans-serif;
  }

  .heading {
    text-align: center;
    margin-bottom: 20px;
  }

  .filter, .sort {
    margin-bottom: 20px;
    display: flex;
    align-items: center;
  }

  .filter-label, .sort-label {
    margin-right: 10px;
  }

  .filter-select, .sort-select {
    padding: 5px;
  }

  .order-list {
    list-style: none;
    padding: 0;
    margin: 0;
  }

  .order-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border: 1px solid #ddd;
    padding: 10px;
    border-radius: 4px;
    margin-bottom: 10px;
  }

  .return-text, .delete-text {
    color: red;
    cursor: pointer;
    font-size: 14px;
    text-decoration: underline;
  }

    .return-text:hover, .delete-text:hover {
      color: darkred;
    }
</style>
