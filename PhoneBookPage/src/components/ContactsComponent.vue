<script setup lang="ts">
import { defineProps, ref, type Ref } from 'vue';
import Panel from 'primevue/panel';
import { Button, Dialog, InputText } from 'primevue';
import { useConfirm } from "primevue/useconfirm";
import { useToast } from "primevue/usetoast";
import ConfirmDialog from 'primevue/confirmdialog';
import Toast from 'primevue/toast';

const confirm = useConfirm();
const toast = useToast();

interface IDataContacts {
  id: number;
  name: string;
  phoneNumber: string;
  email: string;
}

interface IProps {
  dataContacts: IDataContacts[]
}

const visible: Ref<boolean> = ref(false);
const visibleUpdate: Ref<boolean> = ref(false);

const props = defineProps<IProps>();

const contactResult: Ref<IDataContacts | null> = ref(null);

const updateContact: Ref<IDataContacts | null> = ref(null);

const getListById = async (id: number) => {
  await fetch(`http://localhost:5011/api/Contacts/list/${id}`)
    .then(response => response.json())
    .then(actualData => {
      contactResult.value = actualData;
      updateContact.value = actualData;

    })
    .catch((e) => {
      console.error('Das ist der Catch Error: ', e);
      contactResult.value = null; // Em caso de erro, limpa os dados
    });
};

const deleteContact = async (id: number) => {
  await fetch(`http://localhost:5011/api/Contacts/delete/${id}`, {
    method: 'DELETE'
  })
    .then(response => response.json())
    .catch((e) => {
      console.error('Das ist der Catch Error: ', e);
      contactResult.value = null; // Em caso de erro, limpa os dados
    });
};

const updateContacts = () => {
  fetch(`http://localhost:5011/api/Contacts/update/${updateContact.value.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      name: updateContact.value?.name,
      phoneNumber: updateContact.value?.phoneNumber,
      email: updateContact.value?.email,
    }),
  })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok ' + response.statusText);
      }
      return response.json();
    })
    .then(data => {
      console.info('Sucess; ', data)
      window.location.reload();
    })
    .catch((error) => {
      console.error('Error:', error);
    });

}

const dangerMode = (id: number, name: string) => {
  confirm.require({
    message: `Do you want to delete ${name}?`,
    header: 'Danger Zone',
    icon: 'pi pi-info-circle',
    rejectLabel: 'Cancel',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true
    },
    acceptProps: {
      label: 'Delete',
      severity: 'danger'
    },
    accept: async () => {
      toast.add({ severity: 'info', summary: 'Confirmssed', detail: 'Record deleted', life: 3000 });
      setTimeout(() => {
        deleteContact(id)
        window.location.reload();
      }, 1000);
    },
    reject: () => {
      toast.add({ severity: 'error', summary: 'Rejected', detail: 'You have rejected', life: 3000 });
    }
  });
}
</script>

<template>
  <Panel @click="visible = true; getListById(contact.id)" v-for="contact in props.dataContacts" v-bind:key="contact.id"
    :header="contact.name" class="panel">
    <div class="panel-inside">
      <span>Phone Number: {{ contact.phoneNumber }}</span>
      <span>Email: {{ contact.email }}</span>
    </div>
  </Panel>

  <Dialog v-model:visible="visible" modal header="Options" :style="{ width: '25rem' }">
    <template v-if="contactResult">
      <div class="info">
        <span class="text-description">Name: </span>
        <span>{{ contactResult.name }}</span>
      </div>

      <div class="info">
        <span class="text-description">Phone Number: </span>
        <span>{{ contactResult.phoneNumber }}</span>
      </div>

      <div class="info">
        <span class="text-description">Email: </span>
        <span>{{ contactResult.email }}</span>
      </div>

      <Toast />
      <ConfirmDialog></ConfirmDialog>

      <div class="button-container">
        <Button @click="visibleUpdate = true" label="Update" severity="info" />
        <Button @click="dangerMode(contactResult.id, contactResult.name)" label="Delete" severity="danger"
          outlined></Button>
      </div>
    </template>
    <template v-else>
      <span>Loading or no data available...</span>
    </template>
  </Dialog>

  <Dialog v-model:visible="visibleUpdate" modal header="Update" :style="{ width: '25rem' }">
    <div class="input-group">
      <label for="name" class="label">Name</label>
      <InputText v-model="updateContact!.name" id="name" class="input" autocomplete="off" />
    </div>
    <div class="input-group">
      <label for="phoneNumber" class="label">Phone Number</label>
      <InputText v-model="updateContact!.phoneNumber" id="phoneNumber" class="input" autocomplete="off" />
    </div>
    <div class="input-group">
      <label for="email" class="label">Email</label>
      <InputText v-model="updateContact!.email" id="email" class="input" autocomplete="off" />
    </div>
    <div class="button-group">
      <Button type="button" label="Cancel" severity="secondary" @click="visibleUpdate = false"></Button>
      <Button type="button" label="Save" @click="visibleUpdate = false; updateContacts()"></Button>
    </div>
  </Dialog>

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

.input-group {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.label {
  font-weight: 600;
  width: 6rem;
}

.input {
  flex-grow: 1;
}

.button-group {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>