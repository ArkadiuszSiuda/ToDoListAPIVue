import type { Assignment } from '/models/Assignments'

import axios from 'axios'
import type { Ref } from 'vue'

export default class AssignmentService {
  getAllAssignments = async (assignments: Assignment) => {
    const response = await axios.get<Assignment[]>('https://localhost:7000/api/Assignment')
    assignments.value = response.data.map((assignment) => ({
      id: assignment.id,
      name: assignment.name,
      description: assignment.description,
      added: new Date(assignment.added),
      deadline: new Date(assignment.deadline),
      isCompleted: assignment.isCompleted,
    }))
  }

  filterAssignmentsByDate = async (selectedDate: Date, assignments: Assignment) => {
    const year = selectedDate?.getFullYear()
    const month = selectedDate ? selectedDate.getMonth() + 1 : null
    const day = selectedDate?.getDate()
    const date = `${year}-${month}-${day}`

    const response = await axios.get<Assignment[]>(
      `https://localhost:7000/api/Assignment/filter-by-date/${date}`,
    )

    assignments.value = response.data.map((assignment) => ({
      id: assignment.id,
      name: assignment.name,
      description: assignment.description,
      added: new Date(assignment.added),
      deadline: new Date(assignment.deadline),
      isCompleted: assignment.isCompleted,
    }))
  }

  createAssignment = async (assignment: Assignment) => {
    await axios.post('https://localhost:7000/api/Assignment', assignment, {
      headers: {
        'Content-Type': 'application/json',
      },
    })
  }

  updateAssignment = async (data: Ref<Assignment>) => {
    await axios.put('https://localhost:7000/api/Assignment', data.value, {
      headers: {
        'Content-Type': 'application/json',
      },
    })
  }

  changeAssignmentState = async (assignment: Assignment) => {
    assignment.isCompleted = !assignment.isCompleted
    await axios.put('https://localhost:7000/api/Assignment', assignment, {
      headers: {
        'Content-Type': 'application/json',
      },
    })
  }

  deleteAssignment = async (id?: string) =>
    await axios.delete(`https://localhost:7000/api/Assignment/${id}`)
}
