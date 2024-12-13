<script setup lang="ts">


</script>

<template>
    <header class="header">
      <nav class="nav">
        <div class="logo">PhoneBook</div>
        <ul class="nav-links">
          <li><a href="#" class="nav-link">Contact</a></li>
        </ul>
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

.nav-links {
  list-style: none;
  display: flex;
  gap: 20px;
  margin: 0;
  padding: 0;
}

.nav-link {
  text-decoration: none;
  color: #555;
  font-size: 1rem;
  transition: color 0.3s;
}

.nav-link:hover {
  color: #000;
}

@media (max-width: 768px) {
  .nav-links {
    display: none;
    /* You can add a menu icon and toggle logic here for mobile view */
  }
}
</style>