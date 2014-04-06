package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class PreviousButton extends SphericalButton {
    public PreviousButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);
        int width = button.width() - ((strokeWidth << 2) + (padding << 1));
        int halfWidth = width >> 1;
        float startingX = button.centerX() + (halfWidth >> 1);
        float startingY = button.centerY() - halfWidth;

        shape.moveTo(startingX, startingY);
        shape.lineTo(startingX, startingY + width);
        shape.lineTo(startingX - (width - ((halfWidth >> 1) + (strokeWidth << 1))), button.centerY());
        shape.lineTo(startingX, startingY);
        shape.close();
        shape.moveTo(startingX - (width - ((halfWidth >> 1) + strokeWidth)), button.centerY() - halfWidth);
        shape.lineTo(startingX - (width - ((halfWidth >> 1) + strokeWidth)), button.centerY() + halfWidth);
    }

    @Override
    public String command() {
        return PlayerCommand.PREV_CMD;
    }
}
