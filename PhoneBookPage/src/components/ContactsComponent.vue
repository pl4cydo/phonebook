<script setup lang="ts">
import { defineProps } from 'vue';

interface IDataContacts {
    id: number;
    name: string;
    phoneNumber: string;
    email: string;
}

interface IProps {
  dataContacts: IDataContacts
}

const props = defineProps<IProps>();

</script>

<template>
    <Panel v-for="contact in dataContacts" v-bind:key="contact.Id" :header="contact.name" class="panel">
          <div class="panel-inside">
            <span>Phone Number: {{ contact.phoneNumber }}</span>
            <span>Email: {{ contact.email }}</span>
          </div>
    </Panel>
</template>

<style scoped>
.panel-inside {
  display: flex;
  flex-direction: column;
}

.panel-inside>* {
  margin-top: 5px;
}

.panel {
  flex: 1 1 calc(33.333% - 16px);
  box-sizing: border-box;
}
</style>