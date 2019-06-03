// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.querySelectorAll("input[type='checkbox']").forEach(checkbox => {
    checkbox.addEventListener("click", () => {

        const exerciseId = event.target.id.split("-")[1]
        const studentId = event.target.id.split("-")[2]
        const isComplete = event.target.checked
        const studentExercise = { exerciseId, studentId, isComplete }
        console.log(studentExercise)

        fetch(`/Students/CompleteExercise/${studentId}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(studentExercise)
        })   

    })
})