package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 4/18/2014.
 */
public class BringToFrontButton extends PointingUpButton {

    public BringToFrontButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int halfPadding = padding >> 1;
        int x = button.centerX() - halfPadding;

        shape.moveTo(x, button.centerY());
        shape.lineTo(x, button.bottom - padding);

        x = button.centerX() + halfPadding;

        shape.lineTo(x, button.bottom - padding);
        shape.lineTo(x, button.centerY());
        shape.close();
    }

    @Override
    public String command() {
        return PlayerCommand.BING_TO_FRONT_CMD;
    }
}
