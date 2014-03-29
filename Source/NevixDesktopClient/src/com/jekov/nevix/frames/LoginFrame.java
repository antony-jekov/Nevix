package com.jekov.nevix.frames;

import com.intellij.openapi.diagnostic.Log;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

/**
 * Created by Antony Jekov on 3/28/2014.
 */
public class LoginFrame extends JFrame {

    FlowLayout layout;

    public LoginFrame() {
        super("Login");
        this.layout = new FlowLayout();
        setLayout(this.layout);

        JLabel emailLabel = new JLabel("Email");
        final JTextField email = new JTextField(20);
        JLabel passLabel = new JLabel("Password");
        final JTextField pass = new JPasswordField(20);
        final JButton loginBtn = new JButton("Login");
        loginBtn.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (e.getActionCommand().equals(loginBtn.getActionCommand())) {
                    requestLogin(email.getText(), pass.getText());
                }
            }
        });

        add(emailLabel);
        add(email);
        add(passLabel);
        add(pass);
        add(loginBtn);
    }

    private void requestLogin(String email, String pass) {
        System.out.println(email);
        System.out.println(pass);
    }
}
