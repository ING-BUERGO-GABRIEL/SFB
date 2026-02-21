<template>
    <header-bar>
        <search-field style="max-width:300px; width:100%" @search="onSearch" />
        <v-select v-model="codStatus" label="Estado" style="max-width:200px; width:100%" :items="cmbStatus"
            item-title="Description" item-value="Code" placeholder="Seleccionar estado" class="ml-2"
            @update:model-value="onStatusChange" />
        <v-spacer />
    </header-bar>
    <v-row class="mb-0">
        <v-col cols="12">
            <title-card title="Formularios de Examen" class-name="px-0 pb-0 rounded-md">
                <pag-table :headers="headers" :service="tamExamFormServ" :init-params="{ codStatus: 'PEN' }">
                    <template #item.actions="{ item }">
                        <v-btn v-if="item.CodStatus == 'PEN' || item.CodStatus == 'DES'" icon variant="text"
                            color="primary" @click="onClickEdit(item)">
                            <ui-icon name="FormOutlined" size="20" />
                        </v-btn>
                        <v-btn v-if="item.CodStatus == 'PEN'" icon variant="text" color="error"
                            @click="() => onClickDelete(item)">
                            <ui-icon name="DeleteOutlined" size="20" />
                        </v-btn>
                    </template>
                </pag-table>
            </title-card>
        </v-col>
    </v-row>
    <ExamForm ref="examFormRef" />
</template>

<script setup>
import { inject, onMounted, ref } from 'vue'
import ExamForm from './forms/ExamForm.vue'
const { tamExamFormServ } = inject('services')
const { question } = inject('MsgDialog')

const codStatus = ref("PEN")
const cmbStatus = ref([])
const examFormRef = ref(null)

const headers = ref([
    { title: 'Nro. Form', key: 'NroForm' },
    { title: 'Telefono', key: 'PhoneNumber' },
    { title: 'Docente', key: 'NameTeache' },
    { title: 'Tipo Examen', key: 'NameScope' },
    //{ title: 'Estado', key: 'NameStatus' },
    { title: 'Acc.', key: 'actions', sortable: false, align: 'center' }
])

const onSearch = async (filtro) => {
    tamExamFormServ.pageParams.pageNumber = 1
    tamExamFormServ.pageParams.filter = filtro
    await tamExamFormServ.loadPage()
}

const onStatusChange = async (codStatus) => {
    tamExamFormServ.pageParams.pageNumber = 1
    tamExamFormServ.pageParams.codStatus = codStatus
    await tamExamFormServ.loadPage()
}

const onClickDelete = async (item) => {
    const confirmed = await question(
        'Eliminar Formulario',
        `Desea descartar el Formulario`
    )

    if (!confirmed) return null

    await tamExamFormServ.patchUpdate(item.NroForm, { codStatus: 'DES' })
    await tamExamFormServ.loadPage()
}

const onClickEdit = async (item) => {
    const result = await examFormRef.value.openForm('Process', item)
    if (result) {
        await tamExamFormServ.loadPage()
    }
}


onMounted(async () => {
    cmbStatus.value = await tamExamFormServ.getStatus()
})

</script>

<style scoped></style>