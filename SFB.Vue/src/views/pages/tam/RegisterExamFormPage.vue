<template>
    <v-container class="fill-height " style="background-color:#f2ece9" fluid>
        <v-row justify="center">
            <v-col cols="12" style="max-width: 600px">
                <!-- Header Image -->
                <v-card class="mb-4">
                    <!-- Using a placeholder or the original path -->
                    <v-img :src="questionnaireImg" cover height="150" class="bg-grey-lighten-2 rounded-lg">
                        <template v-slot:placeholder>
                            <div class="d-flex align-center justify-center fill-height">
                                <v-icon icon="mdi-image" size="large"></v-icon>
                            </div>
                        </template>
                    </v-img>
                </v-card>

                <!-- Intro Text -->
                <template v-if="!formComplete">
                    <v-card class="mb-4 pa-4">
                        <h2 class="text-h5 mb-4">Propuestas Transformadoras hacia la Calidad Educativa</h2>
                        <v-divider class="mb-4"></v-divider>
                        <p class="mb-4">
                            El siguiente formulario es para solicitar los datos necesarios para crear el documento de
                            PROPUESTAS TRANSFORMADORAS para el examen de ascenso 2025.
                        </p>
                        <v-divider class="mb-4"></v-divider>
                        <span class="text-error text-caption">* Indica que la pregunta es obligatoria</span>
                    </v-card>

                    <!-- Form -->
                    <v-form ref="form" v-model="valid" @submit.prevent="submitForm">

                        <!-- Proposal Title -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Título de su propuesta (Si no tiene título,
                                escriba “Sin título” y elaboraremos uno en función de su contexto.)</label>
                            <v-text-field v-model="formData.proposalTitle" placeholder="Titulo" variant="outlined"
                                hide-details="auto" :rules="[rules.required]"></v-text-field>
                        </v-card>

                        <!-- Scope (Ambito) -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Ambito</label>
                            <v-select v-model="formData.codScope" :items="scopeList" item-title="description"
                                item-value="code" placeholder="Seleccione" variant="outlined" hide-details="auto"
                                :rules="[rules.required]"></v-select>
                        </v-card>

                        <!-- Modality -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Modalidad de Atencion</label>
                            <v-select v-model="formData.codModality" :items="modalityList" item-title="description"
                                item-value="code" placeholder="Seleccione" variant="outlined" hide-details="auto"
                                @update:model-value="onChangedModality" :rules="[rules.required]"></v-select>
                        </v-card>

                        <!-- Area / Level Grid -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Seleccione Nivel</label>
                            <v-data-table v-model="selectedArea" :headers="[{ title: 'Nivel', key: 'description' }]"
                                :items="filteredAreaList" item-value="code" select-strategy="single" show-select
                                class="elevation-0 border" density="compact" items-per-page="-1" hide-default-footer
                                @update:model-value="onSelectionChanged">
                                <template v-slot:no-data>
                                    Seleccione Modalidad de Atencion
                                </template>
                            </v-data-table>
                        </v-card>

                        <!-- School Year (Conditional) -->
                        <v-card v-if="formData.codModality !== 'ADM'" class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Año de Escolaridad</label>
                            <v-radio-group v-model="formData.codSchoolYear" :rules="[rules.required]"
                                hide-details="auto">
                                <v-radio v-for="item in showSchoolYear" :key="item.code" :label="item.description"
                                    :value="item.code"></v-radio>
                            </v-radio-group>
                        </v-card>

                        <!-- Subject / Job Title -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">
                                {{ formData.codModality !== 'ADM' ? "Area o Materia:" : "Especifique su cargo:" }}
                            </label>
                            <v-text-field v-model="formData.subject" placeholder="Escriba" variant="outlined"
                                hide-details="auto" :rules="[rules.required]"></v-text-field>
                        </v-card>

                        <!-- Phone Number -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Número de Teléfono (con el que se contactó con
                                nosotros)</label>
                            <v-text-field v-model="formData.phoneNumber" placeholder="Numero Tel." type="number"
                                variant="outlined" hide-details="auto" :rules="[rules.required]"></v-text-field>
                        </v-card>

                        <!-- Teacher Name -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Nombre del Docente para quien se Relizar la
                                Propuesta :</label>
                            <v-text-field v-model="formData.nameTeache" placeholder="Nombre" variant="outlined"
                                hide-details="auto" :rules="[rules.required]"></v-text-field>
                        </v-card>

                        <!-- School Name -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Nombre de la unidad educativa:</label>
                            <v-text-field v-model="formData.nameSchool" placeholder="Nombre" variant="outlined"
                                hide-details="auto" :rules="[rules.required]"></v-text-field>
                        </v-card>

                        <!-- Department -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Departamento:</label>
                            <v-select v-model="formData.codDepartment" :items="departmentList" item-title="description"
                                item-value="code" placeholder="Seleccione Departamento" variant="outlined"
                                hide-details="auto" :rules="[rules.required]"></v-select>
                        </v-card>

                        <!-- Locality -->
                        <v-card class="mb-4 pa-4">
                            <label class="d-block font-weight-bold mb-2">Distrito o Localidad :</label>
                            <v-text-field v-model="formData.locality" placeholder="Distrito o Localidad"
                                variant="outlined" hide-details="auto" :rules="[rules.required]"></v-text-field>
                        </v-card>

                        <!-- Submit Button -->
                        <div class="mt-6">
                            <v-btn block color="primary" size="large" type="submit" :loading="loadingButton">
                                Enviar
                            </v-btn>
                        </div>

                    </v-form>
                </template>

                <!-- Confirmation -->
                <template v-else>
                    <v-card class="mb-4 pa-4">
                        <h2 class="text-h5 mb-4">Propuestas Transformadoras hacia la Calidad Educativa</h2>
                        <v-divider class="mb-4"></v-divider>
                        <p>¡Muchas gracias por completar el formulario! El proceso de realización del documento se
                            iniciara después del primer Pago.</p>
                    </v-card>
                </template>

            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>
import { ref, computed, reactive, inject } from 'vue'
import questionnaireImg from '@/assets/images/backgrounds/Questionnaire.jpg'
const { examFormServ } = inject('services')

const valid = ref(false)
const loadingButton = ref(false)
const formComplete = ref(false)

// Init Form Data
const formData = reactive({
    proposalTitle: '',
    codScope: null,
    codModality: null,
    codArea: null,
    codSchoolYear: null,
    subject: '',
    phoneNumber: null,
    nameTeache: '',
    nameSchool: '',
    codDepartment: null,
    locality: ''
})

const selectedArea = ref([]) // For Grid Selection

// Validation Rules
const rules = {
    required: value => !!value || 'Este campo es requerido',
}

// --- Data Lists (Transcribed from Sealed C# Classes) ---

const scopeList = [
    { code: 'RUR', description: 'Rural' },
    { code: 'URB', description: 'Urbano' }
]

const modalityList = [
    { code: 'REC', description: 'Regular' },
    { code: 'ALT', description: 'Alternativa' },
    { code: 'ESP', description: 'Especial' },
    { code: 'ADM', description: 'Administrativo' }
]

/* Area.cs Mapping: 
   Code | CodModality | CodSpecialty | Description
   32   | ALT         | ALT          | ALTERNATIVO
   33   | ESP         | ESP          | ESPECIAL
   1    | REC         | INI          | INICIAL
   5    | REC         | PRI          | PRIMARIA
   14   | REC         | SEC          | SECUNDARIA
   6    | ADM         | PRI          | PRIMARIA
   7    | ADM         | SEC          | SECUNDARIA
*/
const areaList = [
    { code: 32, codModality: 'ALT', codSpecialty: 'ALT', description: 'ALTERNATIVO' },
    { code: 33, codModality: 'ESP', codSpecialty: 'ESP', description: 'ESPECIAL' },
    { code: 1, codModality: 'REC', codSpecialty: 'INI', description: 'INICIAL' },
    { code: 5, codModality: 'REC', codSpecialty: 'PRI', description: 'PRIMARIA' },
    { code: 14, codModality: 'REC', codSpecialty: 'SEC', description: 'SECUNDARIA' },
    { code: 6, codModality: 'ADM', codSpecialty: 'PRI', description: 'PRIMARIA' },
    { code: 7, codModality: 'ADM', codSpecialty: 'SEC', description: 'SECUNDARIA' }
]

const schoolYearList = [
    { code: 1, description: 'Primero' },
    { code: 2, description: 'Segundo' },
    { code: 3, description: 'Tercero' },
    { code: 4, description: 'Cuarto' },
    { code: 5, description: 'Quinto' },
    { code: 6, description: 'Sexto' }
]

const departmentList = [
    { code: 'SC', description: 'Santa Cruz' },
    { code: 'CB', description: 'Cochabamba' },
    { code: 'TJ', description: 'Tarija' },
    { code: 'LP', description: 'La Paz' },
    { code: 'PT', description: 'Potosí' },
    { code: 'OR', description: 'Oruro' },
    { code: 'CH', description: 'Chuquisaca' },
    { code: 'BN', description: 'Beni' },
    { code: 'PD', description: 'Pando' }
]

// Logic from Blazor: OnSelectionChanged and filtering
const filteredAreaList = computed(() => {
    if (!formData.codModality) return []
    // Filter areas that match the selected modality
    return areaList.filter(a => a.codModality === formData.codModality)
})

const showSchoolYear = ref([])

function onChangedModality() {
    formData.codArea = null
    formData.codSchoolYear = null
    selectedArea.value = []
    showSchoolYear.value = []
}

function onSelectionChanged(val) {
    // Val is array of selected ids (v-data-table select-strategy="single" returns array)
    if (!val || val.length === 0) {
        formData.codArea = null
        // If deselected, logic clears everything
        formData.codSchoolYear = null
        showSchoolYear.value = []
        return
    }

    const areaId = val[0]
    formData.codArea = areaId
    formData.codSchoolYear = null

    // Logic mimicking switch logic in Blazor (Home.razor)
    const selected = areaList.find(a => a.code === areaId)
    if (!selected) return

    // Logic based on selected Area's Modality (which might be redundant if we filtered by it, but consistent with Blazor)
    const mod = selected.codModality
    const code = selected.code

    if (mod === 'ALT' || mod === 'ESP') {
        // Show all years
        showSchoolYear.value = schoolYearList
    } else if (mod === 'REC') {
        // Case "REC"
        if (code > 0 && code <= 4) {
            // Inicial (1) falls here. In Blazor: ShowSchoolYear = List().Where(s => s.Code == 1 || s.Code == 2);
            showSchoolYear.value = schoolYearList.filter(s => s.code === 1 || s.code === 2)
        } else if (code > 4 && code <= 31) {
            // Primaria (5), Secundaria (14) fall here. Show all.
            showSchoolYear.value = schoolYearList
        } else {
            showSchoolYear.value = []
        }
    } else if (mod === 'ADM') {
        // Case "ADM" -> Auto set to 1, no list shown (since v-if in template hides it)
        formData.codSchoolYear = 1
        showSchoolYear.value = []
    } else {
        showSchoolYear.value = []
    }
}

async function submitForm() {
    if (!valid.value) return
    loadingButton.value = true

    const result = await examFormServ.create(formData)
    if (result) {
        formComplete.value = true
    }
    loadingButton.value = false
}

</script>

<style scoped>
/* Only if absolutely necessary, but user requested Vuetify classes mostly */
.v-card {
    /* Mimic the border-radius 10px from original CSS */
    border-radius: 10px !important;
}
</style>