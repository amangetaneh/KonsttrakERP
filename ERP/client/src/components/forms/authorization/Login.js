import React, { useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { Link } from 'react-router-dom'
import { FcFlowChart } from "react-icons/fc";
import api from '../../../api/api'
import { loginAction } from '../../../redux/auth'
import {ToastContainer,toast} from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css';
import axios from "axios";


const Login = () => {
    const token=useSelector(state=>state.auth.token)
    const storUsername=useSelector(state=>state.auth.username)
    const dispatch = useDispatch()
    console.log(token)
    const [value,setValue] = useState({
        username:"",
        password:""

    })

    const toastOption={
      position: "bottom-right",
      autoClose: 5000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      theme:"dark"
      // progress: undefined
     }
 

    const handleLogin = async(e)=>{
        e.preventDefault()
        if(!value.username){
          toast.error("Please provide your username",toastOption)
        } 
        else if(!value.password){
          toast.error("Please provide your password",toastOption)
        }else{
          try{
            const res = await api.post('Authorization/login',value)
            console.log(res)

            dispatch(loginAction(res.data[0]))
            //dispatch(loginAction(res.data))
            console.log(res.data[0])

            localStorage.setItem("token",res.data[0])
            localStorage.setItem("data",JSON.stringify(res.data))
            //console.log(res.data)
            //axios.defaults.headers.common["Authorization"] = "bearer " + res.data[0]
    
         }catch(err){
           console.log(err)
            toast.error("incorrect username or password",toastOption)
          
         }
        }
        }
     
  return (
    <div className="loginAndRegistration" >
      {storUsername && <div style={{maxWidth:"600px",margin:"auto"}} >
        <div style={{marginTop:"30px"}} class="alert alert-success" role="alert">
     You have successfully registered. Use username: {storUsername} to login.
</div>
        </div>}
    <div>
        

         <form className="login" style={{backgroundColor:"#f1f1f1"}}>
            <h3 style={{color:"grey", paddingTop:"10px"}}><FcFlowChart style={{width: "40px", height: "40px", marginBottom: "10px"}} /> ERP</h3>
                <h3>Login</h3>
                <div className="form-group">
                    <label>Username</label>
                    <input onChange={(e)=>setValue({...value,username:e.target.value})} value={value.username} type="username" className="form-control" placeholder="Enter username" />
                </div>
                <div className="form-group">
                    <label>Password</label>
                    <input onChange={(e)=>setValue({...value,password:e.target.value})} value={value.password} type="password" className="form-control" placeholder="Enter password" />
                </div>
                <div className="form-group">
                    
                </div>
                <button onClick={handleLogin} type="submit" className="btn btn-primary btn-block">Login</button>
                <span className='signup__link'>Not registered? <Link to={"/registration"}>Create account</Link></span>
            </form>
           
    </div>
    <ToastContainer/>
    </div>
  )
}

export default Login