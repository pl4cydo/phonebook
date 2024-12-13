<script setup lang="ts">
import { Drawer } from 'primevue';
import { ref, type Ref } from 'vue';


const visibleRight: Ref<boolean> = ref(false);

</script>

<template>
    <div>
        <ul @click="visibleRight = true" class="nav-links">
          <li><a href="#" class="nav-link">Contact</a></li>
        </ul>
        <Drawer v-model:visible="visibleRight" header="Plácydo Lima" position="right">
            <p>Linkedin: <a href="https://www.linkedin.com/in/placydo/" target="_blank">link</a></p>
            <p>Aplicação feita para test tecnico.</p>
        </Drawer>
    </div>
</template>

<style scoped>
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
  }
}
</style>