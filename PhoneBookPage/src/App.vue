<script setup lang="ts">
// import Card from 'primevue/card';
import Panel from 'primevue/panel';
import { onMounted, ref } from 'vue';
import HeaderComponent from './components/HeaderComponent.vue';
// import HeaderComponent from ';

const dataContacts = ref(null)

const getList = () => {
  fetch('http://localhost:5011/api/Contacts/list')
    .then(response => response.json())
    .then(actualData => {
      dataContacts.value = actualData
    })
    .catch((e) => {
      console.error('Das ist der Catch Error: ', e)
    })
}

onMounted(() => {
  getList()
})

</script>

<template>
  <div class="container">
    <HeaderComponent />

    <div class="page">
      <div class="top-bar">
        <h3>List of Contacts</h3>
        <h3>A - Z</h3>
      </div>
      <div class="panel-container">
        
      </div>
    </div>
  </div>
</template>

<style scoped>
.container {
  padding: 0;
  margin: 0;
  width: 100wv;
  height: auto;
  display: flex;
  justify-content: center;
}

.page {
  background-color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 10px 20px;
  top: 0;
  width: 80%;
  display: flex;
  flex-direction: column;
  margin-top: 55px;
  max-width: 1200px;
  padding-bottom: 40px;
}

.top-bar {
  display: flex;
  justify-content: space-between;
}

.panel-container {
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
}
</style>