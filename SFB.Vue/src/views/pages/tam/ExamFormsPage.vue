<template>
    <header-bar>
        <search-field style="max-width:300px; width:100%" @search="onSearch" />
        <v-select v-model="codStatus" label="Estado" style="max-width:200px; width:100%" :items="cmbStatus"
            item-title="Description" item-value="Code" placeholder="Seleccionar estado" class="ml-2"
            @update:model-value="onSearch" />
        <v-spacer />
    </header-bar>
    <v-row class="mb-0">
        <v-col cols="12">
            <title-card title="Formularios de Examen" class-name="px-0 pb-0 rounded-md">
                <pag-table :headers="headers" :service="examFormServ" :init-params="{ codStatus: 'PEN' }">
                    <template #item.actions="{ item }">
                        <v-btn icon variant="text" color="primary" @click="emit('edit', item)">
                            <ui-icon name="UnorderedListOutlined" size="20" />
                        </v-btn>
                    </template>
                </pag-table>
            </title-card>
        </v-col>
    </v-row>
</template>

<script setup>
import { inject, onMounted, ref } from 'vue'
const { examFormServ } = inject('services')

const codStatus = ref("PEN")
const cmbStatus = ref([])

const headers = ref([
    { title: 'Nro.', key: 'NroForm' },
    { title: 'Telefono', key: 'PhoneNumber' },
    { title: 'Docente', key: 'NameTeache' },
    { title: 'Departamento', key: 'NameDepartment' },
    { title: 'Area', key: 'NameScope' },
    { title: 'Modalidad', key: 'NameModality' },
    { title: 'Curso', key: 'NameSchoolYear' },
    { title: 'Acc.', key: 'actions', sortable: false, align: 'center' }
])

const onSearch = async (filtro) => {
    examFormServ.pageParams.pageNumber = 1
    examFormServ.pageParams.filter = filtro
    await examFormServ.loadPage()
}

onMounted(async () => {
    cmbStatus.value = await examFormServ.getStatus()
})

</script>

<style scoped></style>