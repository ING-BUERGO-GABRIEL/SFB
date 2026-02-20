<template>
    <dialog-body v-model="showModal" :title="titleDlg" formValidate @accept="onAccept" @cancel="onCancel">
        <template #body>
            <v-row class="mb-0" justify="center">
                <!-- Columna Izquierda: Datos del Docente y Task -->
                <v-col cols="12" md="6">
                    <parent-card :showHeader="true" title="Datos del Docente">
                        <v-row class="pa-4">
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field label="Docente" v-model="formTeacher.NameTeache" readonly
                                        density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field label="Telefono" v-model="formTeacher.PhoneNumber" readonly
                                        density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field v-model="teacherTask.PriceTotal" label="Precio Total"
                                        :rules="[rRequired]" type="number" step="0.01" required density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field v-model="teacherTask.AmountPaid" label="Monto Cobrado" type="number"
                                        step="0.01" density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" class="py-0">
                                <div class="mb-6">
                                    <v-text-field v-model="teacherTask.NameContact" label="Nombre de Contacto"
                                        :rules="[rRequired]" required placeholder="X001..." density="compact" />
                                </div>
                            </v-col>
                        </v-row>
                    </parent-card>
                </v-col>

                <!-- Columna Derecha: Detalles de la Tarea -->
                <v-col cols="12" md="6">
                    <parent-card :showHeader="true" title="Detalles de la Tarea">
                        <v-row class="pa-4">
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-select v-model="teacherTask.NroPersonReturnString"
                                        label="Responsable de Devolucion" :items="personReturnList" :rules="[rRequired]"
                                        item-title="Name" item-value="NroPerson" required placeholder="Seleccione"
                                        density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-select v-model="teacherTask.NroPersonAssignedString" label="Responsable Actual"
                                        :items="personAsignetList" item-title="Name" item-value="NroPerson"
                                        placeholder="Ninguno" density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-select v-model="teacherTask.CodStatus" label="Estado de la Tarea"
                                        :items="statusList" :rules="[rRequired]" item-title="Description"
                                        item-value="Code" required placeholder="Estado" density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field label="Fecha de Registro" v-model="today" readonly
                                        density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field v-model="deliveryDate" label="Fecha de Entrega" :rules="[rRequired]"
                                        type="date" required density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" sm="6" class="py-0">
                                <div class="mb-6">
                                    <v-text-field v-model="deliveryTime" label="Hora" :rules="[rRequired]" type="time"
                                        required density="compact" />
                                </div>
                            </v-col>
                            <v-col cols="12" class="py-0">
                                <div class="mb-6">
                                    <v-text-field v-model="teacherTask.UrlDocument" label="Url de Documento"
                                        :rules="[rRequired]" required placeholder="https://" density="compact" />
                                </div>
                            </v-col>
                        </v-row>
                    </parent-card>
                </v-col>
            </v-row>
        </template>
    </dialog-body>
</template>

<script setup>
import { ref, inject, computed } from 'vue'

const { examFormServ, uiStore, amsPersonServ } = inject('services')
import { message } from 'ant-design-vue'

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('Registrar Trabajo')
const teacherTask = ref({})
const formTeacher = ref({})

const personReturnList = ref([])
const personAsignetList = ref([])
const statusList = ref([])

const deliveryDate = ref('')
const deliveryTime = ref('')

const today = computed(() => {
    const d = new Date()
    return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`
})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'

const teacherTaskDefault = () => {
    return {
        NroTeacherTask: 0,
        NroFromTeacher: 0,
        PriceTotal: 0,
        AmountPaid: 0,
        NameContact: '',
        NroPersonReturnString: null,
        NroPersonAssignedString: null,
        CodStatus: 'PEN',
        DeliveryDate: null,
        UrlDocument: ''
    }
}

let _resolve = null

async function openForm(mode, item = null) {
    modeDlg.value = mode
    uiStore.isLoadingBody = true

    await loadMetadata()

    if (mode === 'Process') {
        titleDlg.value = 'Registrar Trabajo'
        formTeacher.value = item
        teacherTask.value = teacherTaskDefault()
        teacherTask.value.NroFromTeacher = item.NroForm

        // Set default delivery date (3 days later as in Razor)
        const future = new Date()
        future.setDate(future.getDate() + 3)
        deliveryDate.value = future.toISOString().split('T')[0]
        deliveryTime.value = future.toTimeString().split(' ')[0].substring(0, 5)

        showModal.value = true
    } else if (mode === 'Update') {
        titleDlg.value = 'Editar Trabajo'
        teacherTask.value = JSON.parse(JSON.stringify(item))
        formTeacher.value = item.FormTeacher || {}

        if (teacherTask.value.DeliveryDate) {
            const d = new Date(teacherTask.value.DeliveryDate)
            deliveryDate.value = d.toISOString().split('T')[0]
            deliveryTime.value = d.toTimeString().split(' ')[0].substring(0, 5)
        }

        showModal.value = true
    }

    uiStore.isLoadingBody = false

    return new Promise(resolve => {
        _resolve = resolve
    })
}

async function loadMetadata() {
    // Assuming getMetadata returns an object with Persons and Statuses
    //const meta = await examFormServ.getMetadata()

    personReturnList.value = await amsPersonServ.getPersonList()

    // const listWithNone = JSON.parse(JSON.stringify(meta.Persons || []))
    // listWithNone.push({ NroPerson: 0, Name: 'Ninguno' })
    // personAsignetList.value = listWithNone

    // // If statuses are not in metadata, use getStatus
    // if (meta.Statuses) {
    //     statusList.value = meta.Statuses
    // } else {
    //     statusList.value = await examFormServ.getStatus()
    // }
}

async function onAccept() {
    // Combine date and time
    const combinedDateTime = `${deliveryDate.value}T${deliveryTime.value}:00`
    teacherTask.value.DeliveryDate = combinedDateTime

    uiStore.isLoadingBody = true
    try {
        let result = null
        if (modeDlg.value === 'Process') {
            // In blazor it calls TeacherTaskService.Create
            // We'll use examFormServ for now, assuming it has the method or the backend route matches
            result = await examFormServ.create(teacherTask.value)
        } else if (modeDlg.value === 'Update') {
            result = await examFormServ.update(teacherTask.value)
        }

        if (result) {
            _resolve(result)
            showModal.value = false
        }
    } catch (error) {
        message.error('Error al guardar: ' + error)
    } finally {
        uiStore.isLoadingBody = false
    }
}

function onCancel() {
    _resolve(null)
    showModal.value = false
}

defineExpose({
    openForm
})
</script>
