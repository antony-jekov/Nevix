package com.antonyjekov.nevix.common;

/**
 * Created by Antony Jekov on 4/2/2014.
 */
public class MediaItem {
    public MediaItem(String title, int iconResourceId, int position) {
        this.title = title;
        this.iconResourceId = iconResourceId;
    }

    public int iconResourceId;
    public String title;
    public int position;
}
