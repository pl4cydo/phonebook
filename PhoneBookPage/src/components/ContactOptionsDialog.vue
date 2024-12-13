<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import { Button, Dialog, ConfirmDialog, Toast } from 'primevue';

interface IDataContacts {
    id: number;
    name: string;
    phoneNumber: string;
    email: string;
}

interface IProps {
    visible: boolean;
    contact: IDataContacts;
}

const props = defineProps<IProps>();

defineEmits(['update', 'delete']);
</script>

<template>
    <Dialog v-model:visible="props.visible" modal header="Options" :style="{ width: '25rem' }">
        <template v-if="contact">
            <div class="info">
                <span class="text-description">Name: </span>
                <span>{{ contact.name }}</span>
            </div>

            <div class="info">
                <span class="text-description">Phone Number: </span>
                <span>{{ contact.phoneNumber }}</span>
            </div>

            <div class="info">
                <span class="text-description">Email: </span>
                <span>{{ contact.email }}</span>
            </div>

            <Toast />
            <ConfirmDialog />

            <div class="button-container">
                <Button @click="$emit('update')" label="Update" severity="info" />
                <Button @click="$emit('delete', contact.id, contact.name)" label="Delete" severity="danger" outlined />
            </div>
        </template>
        <template v-else>
            <span>Loading or no data available...</span>
        </template>
    </Dialog>
</template>

<style scoped>
.info {
    display: flex;
    margin-bottom: 15px;
}

.info>* {
    margin-inline: 2px;
}

.button-container {
    width: 100%;
    display: flex;
    justify-content: end;
}

.button-container>* {
    margin-inline: 5px;
}
</style>