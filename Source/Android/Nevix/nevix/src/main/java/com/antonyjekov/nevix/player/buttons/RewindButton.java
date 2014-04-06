package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class RewindButton extends SphericalButton {
    public RewindButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int size = button.width() >> 1;

        float startX = button.centerX() + (size >> 1);
        float startY = button.centerY() - (size >> 1);

        shape.moveTo(startX, startY);
        shape.lineTo(startX, startY + size);
        shape.lineTo(startX - (size >> 1), button.centerY());
        shape.lineTo(startX, startY);
        shape.close();
        startX = startX - ((size >> 1) + strokeWidth);
        shape.moveTo(startX, startY);
        shape.lineTo(startX, startY + size);
        shape.lineTo(startX - (size >> 1), button.centerY());
        shape.lineTo(startX, startY);
        shape.close();
    }

    @Override
    public String command() {
        return PlayerCommand.RW_CMD;
    }
}
