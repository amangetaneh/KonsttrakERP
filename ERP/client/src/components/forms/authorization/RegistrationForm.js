import React, { useState } from 'react'

const RegistrationForm = ()=>{
    const [username,setUsername] = useState("")
    const [password,setPassword] = useState("")
    const [firstname,setFirst] = useState("")
    const [latname,setLastName] = useState("")
    const [middlename,setMiddlename] = useState("")
    const [postion,setPostion] = useState("")


    return(
        <div>
            <input type={"text"}/>
            <input type={"text"}/>
            <input type={"number"}/>
            <input type={"text"}/>
            <input type={"text"}/>
            <input type={"text"}/>
            <input type={"text"}/>


        </div>
    )
}