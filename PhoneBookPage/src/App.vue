<script setup lang="ts">
import { onMounted, ref, Ref } from 'vue';
import HeaderComponent from './components/HeaderComponent.vue';
import ContactsComponent from './components/ContactsComponent.vue';
import AddContact from './components/AddContact.vue';


interface IDataContacts {
    id: number;
    name: string;
    phoneNumber: string;
    email: string;
}

const dataContacts: Ref<IDataContacts[]> = ref(null)

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

const createContact = () => {

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
        <h3>List of Contacts  A - Z</h3>
        <AddContact />
      </div>
      <div class="panel-container">
        <ContactsComponent :dataContacts="dataContacts"/>
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
  align-items: center;
}

.top-bar > * {
  margin-inline: 20px;
}

.panel-container {
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
}
</style>