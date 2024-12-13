<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import Panel from 'primevue/panel';

interface IProp {
  contact: {
    id: number;
    name: string;
    phoneNumber: string;
    email: string;
  }
}

const props = defineProps<IProp>();

defineEmits(['select']);
</script>

<template>
  <Panel @click="$emit('select', props.contact.id)" :header="props.contact.name" class="panel">
    <div class="panel-inside">
      <span>Phone Number: {{ props.contact.phoneNumber }}</span>
      <span>Email: {{ props.contact.email }}</span>
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
  cursor: pointer;
  transition: all 0.3s ease;
}

.panel:hover {
  background-color: #eef5fc;
}
</style>