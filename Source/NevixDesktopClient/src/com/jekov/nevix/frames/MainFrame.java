package com.jekov.nevix.frames;

import com.jekov.nevix.common.DataManager;
import com.jekov.nevix.common.Persister;

import javax.swing.*;
import java.awt.*;

/**
 * Created by Antony Jekov on 3/28/2014.
 */
public class MainFrame extends JFrame {

    FlowLayout layout;

    public MainFrame() {
        super("Nevix - Desktop Client");

        this.layout = new FlowLayout();
        setLayout(layout);

        String[] mappedFolders = DataManager.instance().mappedFolders();

        Persister.instance().test();
    }
}
