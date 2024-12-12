<script setup lang="ts">
import { ref, onMounted } from 'vue'
import AssignmentService from '../services/AssignmentService'
import type { Assignment } from '../models/Assignment'
import InputText from 'primevue/inputtext'
import InputTextarea from 'primevue/textarea'
import Calendar from 'primevue/calendar'
import Button from 'primevue/button'
import Card from 'primevue/card'

const tasksWithOneDayLeft = ref<Assignment[]>([])
const assignmentService = new AssignmentService()
const selectedDate = ref<Date>()
const assignments = ref<Assignment[]>([])
const assignment = ref<Assignment>({
  id: undefined,
  name: '',
  description: '',
  added: new Date(),
  deadline: new Date(),
  isCompleted: false,
})
const isEditing = ref(false)
const currentEditingId = ref<string | null>()
const isAdding = ref(false)

onMounted(() => {
  handleGetAllAssignments().then(() => {
    checkTasksWithOneDayLeft()
  })
})

const handleGetAllAssignments = () => assignmentService.getAllAssignments(assignments)

const handleCreateAssignment = () => {
  assignmentService.createAssignment(assignment.value).then(() => {
    isAdding.value = false
    resetData(assignment.value)
    handleGetAllAssignments()
  })
}

const handleUpdateAssignment = () => {
  if (assignment.value.id) {
    assignmentService.updateAssignment(assignment).then(() => {
      isEditing.value = false
      currentEditingId.value = null
      resetData(assignment.value)
      handleGetAllAssignments()
    })
  }
}

const handleDeleteAssignment = (id?: string) => {
  assignmentService.deleteAssignment(id).then(() => {
    handleGetAllAssignments()
  })
}

const handleFilterAssignmentsByDate = () =>
  assignmentService.filterAssignmentsByDate(selectedDate.value ?? new Date(), assignments)

const handleChangeAssignmentState = (assignment: Assignment) => {
  assignmentService.changeAssignmentState(assignment).then(() => {
    handleGetAllAssignments()
  })
}

const checkTasksWithOneDayLeft = () => {
  const now = new Date()
  const today = new Date(now.getFullYear(), now.getMonth(), now.getDate())
  tasksWithOneDayLeft.value = assignments.value.filter((task) => {
    if (task.isCompleted) {
      return false
    }
    const deadline = new Date(task.deadline)
    const taskDeadline = new Date(deadline.getFullYear(), deadline.getMonth(), deadline.getDate())
    return taskDeadline.getTime() <= today.getTime()
  })
}

const getSortedData = () => {
  return tasksWithOneDayLeft.value
    .sort((a, b) => {
      const deadlineA = new Date(a.deadline)
      const deadlineB = new Date(b.deadline)
      return deadlineA.getTime() - deadlineB.getTime()
    })
    .slice(0, 5)
}

const resetData = async (assignment: Assignment) => {
  assignment.id = undefined
  assignment.name = ''
  assignment.description = ''
  assignment.added = new Date()
  assignment.deadline = new Date()
  assignment.isCompleted = false
}

const startEditing = (assignmentEdit: Assignment) => {
  assignment.value = { ...assignmentEdit }
  isEditing.value = true
  currentEditingId.value = assignmentEdit.id
}

const cancelEditing = () => {
  isEditing.value = false
  currentEditingId.value = null
}

const startAdding = () => (isAdding.value = true)

const cancelAdding = () => (isAdding.value = false)
</script>

<template>
  <header>
    <nav>
      <div class="date-picker">
        <label for="date"><i class="pi pi-calendar"></i> Wybierz datę:</label>
        <Calendar
          v-model="selectedDate"
          id="date"
          dateFormat="yy-mm-dd"
          placeholder="Wybierz datę"
          :showIcon="false"
          @update:modelValue="handleFilterAssignmentsByDate"
        />
      </div>
    </nav>
  </header>

  <h2 class="title">Lista zadań:</h2>

  <div v-if="tasksWithOneDayLeft.length > 0" class="overlay">
    <div class="overlay-content">
      <h3>Ilość zadań: {{ tasksWithOneDayLeft.length > 5 ? '5+' : tasksWithOneDayLeft.length }}</h3>
      <ul>
        <li v-for="task in getSortedData()" :key="task.id">
          {{ task.name }} - Termin: {{ new Date(task.deadline).toLocaleString() }}
        </li>
      </ul>
      <Button label="Zamknij" @click="tasksWithOneDayLeft = []" class="p-button-danger"></Button>
    </div>
  </div>

  <div class="card-container">
    <Card
      v-for="card in assignments"
      :key="card.id"
      class="card"
      :class="{
        'editing-card': isEditing && currentEditingId === card.id,
        'completed-card': card.isCompleted,
      }"
    >
      <template #title>
        <h2 class="card-title">{{ card.name }}</h2>
      </template>

      <template #content>
        <div v-if="isEditing && currentEditingId === card.id">
          <div class="p-field">
            <label for="name">Nazwa zadania</label>
            <InputText v-model="assignment.name" id="name" required />
          </div>
          <div class="p-field">
            <label for="description">Opis zadania</label>
            <InputTextarea v-model="assignment.description" id="description" rows="4" required />
          </div>
          <div class="p-field">
            <label for="deadline">Termin wykonania</label>
            <Calendar
              v-model="assignment.deadline"
              id="deadline"
              showTime
              placeholder="Wybierz datę zakończenia"
            />
          </div>
        </div>
        <div v-else>
          <div class="description-container">
            <p>{{ card.description }}</p>
          </div>
          <p><span class="date-font">Dodano:</span> {{ new Date(card.added).toLocaleString() }}</p>
          <p>
            <span class="date-font">Termin:</span> {{ new Date(card.deadline).toLocaleString() }}
          </p>
        </div>
      </template>

      <template #footer>
        <div class="footer-container">
          <template v-if="!isEditing || currentEditingId !== card.id">
            <Button
              @click="startEditing(card)"
              v-tooltip.bottom="{ value: 'Edytuj', showDelay: 500, hideDelay: 150 }"
              icon="pi pi-pencil"
              class="p-button-icon"
            ></Button>
            <Button
              @click="handleDeleteAssignment(card.id)"
              v-tooltip.bottom="{ value: 'Usuń', showDelay: 500, hideDelay: 150 }"
              icon="pi pi-trash"
              class="p-button-danger"
            ></Button>
            <Button
              v-if="!card.isCompleted"
              @click="handleChangeAssignmentState(card)"
              v-tooltip.bottom="{ value: 'Ukończ', showDelay: 500, hideDelay: 150 }"
              icon="pi pi-check"
              class="p-button-success"
            ></Button>
            <Button
              v-else
              @click="handleChangeAssignmentState(card)"
              v-tooltip.bottom="{ value: 'Cofnij', showDelay: 500, hideDelay: 150 }"
              icon="pi pi-replay"
              class="p-button-danger"
            ></Button>
          </template>
          <template v-else>
            <Button
              label="Anuluj"
              icon="pi pi-times"
              class="p-button-secondary"
              @click="cancelEditing"
            ></Button>
            <Button
              label="Akceptuj"
              icon="pi pi-check"
              class="p-button-primary"
              @click="handleUpdateAssignment"
            ></Button>
          </template>
        </div>
      </template>
    </Card>

    <div v-if="!isAdding" class="add-card" @click="startAdding">
      <i class="pi pi-plus"></i>
    </div>

    <Card v-if="isAdding" class="card editing-card">
      <template #title>
        <h3>Dodaj nowe zadanie</h3>
      </template>

      <template #content>
        <div class="p-field">
          <label for="name">Nazwa zadania</label>
          <InputText
            v-model="assignment.name"
            id="name"
            required
            placeholder="Wpisz nazwę zadania"
          />
        </div>
        <div class="p-field">
          <label for="description">Opis zadania</label>
          <InputTextarea
            v-model="assignment.description"
            id="description"
            rows="4"
            required
            placeholder="Wpisz opis zadania"
          />
        </div>
        <div class="p-field">
          <label for="deadline">Termin wykonania</label>
          <Calendar
            v-model="assignment.deadline"
            id="deadline"
            required
            showTime
            placeholder="Wybierz termin wykonania"
          />
        </div>
      </template>

      <template #footer>
        <Button
          label="Stwórz"
          icon="pi pi-check"
          class="p-button-primary"
          @click="handleCreateAssignment"
        ></Button>
        <Button
          label="Anuluj"
          icon="pi pi-times"
          class="p-button-secondary"
          @click="cancelAdding"
        ></Button>
      </template>
    </Card>
  </div>
</template>
