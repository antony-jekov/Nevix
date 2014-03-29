package com.jekov.nevix.common;

/**
 * Created by Antony Jekov on 3/28/2014.
 */
public class DataManager {
    private static DataManager instance = new DataManager();

    public static DataManager instance() {
        return instance;
    }

    public String[] mappedFolders() {
        String[] folders = new String[]{"Batman", "Zoro"};

        return folders;
    }

}
