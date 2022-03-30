import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import {register} from '../api/authorization'

export const authSlice= createSlice({
  name: 'counter',
  initialState:{
   token:"",
   username:""
  },
  reducers: {
    
    loginAction: (state,action) => {
       state.token=action.payload
    },
    registration:(state,action)=>{
      state.username = action.payload
    }
}
})

// Action creators are generated for each case reducer function
export const {  loginAction,registration } = authSlice.actions

export default authSlice.reducer