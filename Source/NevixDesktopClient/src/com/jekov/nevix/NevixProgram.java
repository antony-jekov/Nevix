package com.jekov.nevix;

import com.jekov.nevix.frames.LoginFrame;
import com.jekov.nevix.frames.MainFrame;

import javax.swing.*;

/**
 * Created by Antony Jekov on 3/28/2014.
 */
public class NevixProgram {
    public static void main(String[] args) {
        //displayLoginFrame();
        displayMainFrame();
    }

    private static void displayLoginFrame() {
        JFrame loginFrame = new LoginFrame();
        loginFrame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);

        loginFrame.setSize(275, 180);
        loginFrame.setVisible(true);
    }

    private static void displayMainFrame() {
        JFrame mainFrame = new MainFrame();
        mainFrame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);

        mainFrame.setSize(640, 480);
        mainFrame.setVisible(true);
    }
}
