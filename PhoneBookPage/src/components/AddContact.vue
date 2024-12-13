    <script setup lang="ts">
    import { Button, InputText } from 'primevue';
    import Dialog from 'primevue/dialog';
    import { ref, type Ref } from 'vue';

    interface IContact {
        name: string;
        phoneNumber: string;
        email: string;
    }

    const newContact: Ref<IContact> = ref({
        name: '',
        phoneNumber: '',
        email: ''
    })
    const visible: Ref<boolean> = ref(false);

    const createContact = () => {
        console.log(newContact)
        fetch('http://localhost:5011/api/Contacts/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json', 
            },
            body: JSON.stringify(newContact.value),
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok ' + response.statusText);
                }
                return response.json();
            })
            .then(data => {
                console.info('Sucess: ', data)
                window.location.reload();
            })
            .catch((error) => {
                console.error('Error:', error);
            });

    }

    </script>

    <template>
        <div>
            <img @click="visible = true" alt="plus" class="plus" src="../assets/plus-svgrepo-com.svg" />
            <Dialog v-model:visible="visible" modal header="New Contact" :style="{ width: '25rem' }">
                <span class="text-description">Add new information</span>
                <div class="input-group">
                    <label for="name" class="label">Name</label>
                    <InputText v-model="newContact.name" id="name" class="input" autocomplete="off" />
                </div>
                <div class="input-group">
                    <label for="phoneNumber" class="label">Phone Number</label>
                    <InputText v-model="newContact.phoneNumber" id="phoneNumber" class="input" autocomplete="off" />
                </div>
                <div class="input-group">
                    <label for="email" class="label">Email</label>
                    <InputText v-model="newContact.email" id="email" class="input" autocomplete="off" />
                </div>
                <div class="button-group">
                    <Button type="button" label="Cancel" severity="secondary" @click="visible = false"></Button>
                    <Button type="button" label="Save" @click="visible = false; createContact()"></Button>
                </div>
            </Dialog>
        </div>
    </template>


    <style scoped>
    .text-description {
        color: #6b7280;
        color: #9ca3af;
        display: block;
        margin-bottom: 2rem;
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
        /* w-24 */
    }

    .input {
        flex-grow: 1;
    }

    .button-group {
        display: flex;
        justify-content: flex-end;
        gap: 0.5rem;
    }

    .plus {
        width: 30px;
        height: 30px;
        color: #555;
        transition: all 0.3s ease;
        cursor: pointer;
    }

    .plus:hover {
        width: 35px;
        height: 35px;
    }
    </style>