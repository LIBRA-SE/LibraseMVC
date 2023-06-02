

const Excluir = document.querySelector("#excluir")
const modal = document.querySelector("dialog")

const buttonClose = document.querySelector("#fechar")
console.log(Excluir, buttonClose)

Excluir.onclick = function () {
    modal.showModal()
}


buttonClose.onclick = function () {
    modal.close()
}