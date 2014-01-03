package com.antonyjekov.nevix.models;

public class UserLoginModel {
    private String email;
    private String password;

    public UserLoginModel(String email, String pass){
        this.email = email;
        this.password = pass;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
