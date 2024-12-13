<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import { InputText, Button, Dialog } from 'primevue';

interface IDataContacts {
    name: string;
    phoneNumber: string;
    email: string;
}

interface IProps {
  visible: boolean,
  contact: IDataContacts,
}

const props = defineProps<IProps>();

defineEmits(['save', 'cancel']);
</script>

<template>
    <Dialog v-model:visible="props.visible" modal header="Update" :style="{ width: '25rem' }">
      <div class="input-group">
        <label for="name" class="label">Name</label>
        <InputText v-model="props.contact.name" id="name" class="input" autocomplete="off" />
      </div>
      <div class="input-group">
        <label for="phoneNumber" class="label">Phone Number</label>
        <InputText v-model="props.contact.phoneNumber" id="phoneNumber" class="input" autocomplete="off" />
      </div>
      <div class="input-group">
        <label for="email" class="label">Email</label>
        <InputText v-model="props.contact.email" id="email" class="input" autocomplete="off" />
      </div>
      <div class="button-group">
        <Button type="button" label="Cancel" severity="secondary" @click="$emit('cancel')" />
        <Button type="button" label="Save" @click="$emit('save')" />
      </div>
    </Dialog>
  </template>
  
  <style scoped>
  .input-group {
    margin-bottom: 15px;
  }
  
  .label {
    font-weight: bold;
  }
  
  .button-group {
    display: flex;
    justify-content: space-between;
  }
  </style>