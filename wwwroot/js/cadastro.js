//Validações campos

const form = document.getElementById('form');
const campos = document.querySelectorAll('.required');
const spans = document.querySelectorAll('.span-required ')

const numeros = /[01234567890]/
const caractereEspecial = /^(?:\d{3}|\(\d{3}\))([-/.])\d{3}\1\d{4}$/;


function setError(index) {
    campos[index].style.border = '2px solid #e63636'
    spans[index].style.display = 'block';
}

function nameValidate() {

    if (campos[0].value.length < 5 || campos[0].value.length >20) {
        setError(0);
    }
    else {
        removerErro(0)
    }

}

//function emailValidate() {

//    if

//}

function removerErro(index) {

    campos[index].style.border = '';
    spans[index].style.display = 'none';

}

function mainPasswordValidate(){

    if(campos[1].value.length <6 || campos[1].value.length >25){
        setError(1);
    }else{

        if(numeros.test(campos[1].value)){
            removerErro(1);
        return true
        } else{
            setError(1)
        }
       
        comparePassword();
    }

}

function comparePassword() {
    if (campos[1].value == campos[2].value && campos[2].value.length >= 8) {
        removerErro(2);
    } else {
        setError(2);
    }
}


