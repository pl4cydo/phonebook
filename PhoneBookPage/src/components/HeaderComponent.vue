<script setup lang="ts">
import DrawerComponent from './DrawerComponent.vue';

</script>

<template>
    <header class="header">
      <nav class="nav">
        <div class="logo">PhoneBook</div>
        <DrawerComponent />
      </nav>
    </header>
</template>

<style scoped>
    .header {
  background-color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 10px 20px;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  z-index: 1000;
}

.nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto;
}

.logo {
  font-size: 1.5rem;
  font-weight: bold;
  color: #333;
}
</style>